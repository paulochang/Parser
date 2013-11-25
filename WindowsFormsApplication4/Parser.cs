using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parser
{
    class Parser
    {
        List<Token> tokenList;
        List<Token>.Enumerator tokenEnumerator;
        MainForm form;

        List<LexerCharacterSet> characterDefinitions = new List<LexerCharacterSet>();
        List<LexerKeyword> keywordDefinitions = new List<LexerKeyword>();
        List<LexerToken> tokenDefinitions = new List<LexerToken>();
        List<ProductionClass> parserDefinitions = new List<ProductionClass>();
        LexerCharacterSet whitespaceDefinitions = new LexerCharacterSet();

        public Parser(List<Token> theList)
        {
            tokenList = theList;
            tokenEnumerator = tokenList.GetEnumerator();
            tokenEnumerator.MoveNext();
        }

        public void Parse(MainForm theForm)
        {
            form = theForm;
            if (Cocol())
                form.appendText("FILE PARSED CORRECTLY!! \n");
            else
                form.appendText("COULDN'T PARSE FILE!!! \n");
            form.appendText("This keywords where found: \n");
            foreach (LexerKeyword theToken in keywordDefinitions)
            {
                form.appendText(theToken.Identifier + " = " + theToken.Regexp + " \n");
            }
            form.appendText("This characterSets where found: \n");
            foreach (LexerCharacterSet theToken in characterDefinitions)
            {
                form.appendText(theToken.Identifier + " = " + theToken.Regexp + " \n");
            }
            form.appendText("This tokens where defined: \n");
            foreach (LexerToken theToken in tokenDefinitions)
            {
                form.appendText(theToken.Identifier + " = " + theToken.Regexp + " \n");
            }
            form.appendText("This characters are whitespace: \n");
            form.appendText(whitespaceDefinitions.Identifier + " = " + whitespaceDefinitions.Regexp + " \n");
            form.setLexer(characterDefinitions, keywordDefinitions, tokenDefinitions, whitespaceDefinitions);
        }

        bool term(TokenType tok, bool consuming)
        {
            bool result = true;
            TokenType next = tokenEnumerator.Current.getTokenType();
            result = (next == tok);
            if (consuming)
            {
                if (result == false)
                {
                    MessageBox.Show("Error!!! " + tok + " Expected");
                    MessageBox.Show(tokenEnumerator.Current.getTokenType() + " with value " + tokenEnumerator.Current.getTokenType() + " found instead.");
                }
                tokenEnumerator.MoveNext();
            }
            return result;
        }

        bool Cocol()
        {
            form.appendText("parsing cocol \n");
            return term(TokenType.COMPILER, true) &&
                term(TokenType.Ident, true) &&
                ScannerSpecification() &&
                ParserSpecification() &&
                term(TokenType.END, true) &&
                term(TokenType.Ident, true) &&
                term(TokenType.DOT, true);
        }

        bool ScannerSpecification()
        {
            bool result = true;
            if (term(TokenType.CHARACTERS, false))
            {
                result = result && term(TokenType.CHARACTERS, true);
                while (term(TokenType.Ident, false))
                {
                    LexerCharacterSet tmpSet = new LexerCharacterSet();
                    result = result && SetDecl(out tmpSet);
                    characterDefinitions.Add(tmpSet);
                }
                form.appendText("parsed CHARACTERSE\n");
            }

            if (term(TokenType.KEYWORDS, false))
            {
                result = result && term(TokenType.KEYWORDS, true);
                while (term(TokenType.Ident, false))
                {
                    result = result && KeywordDecl();
                }
                form.appendText("parsed KEYWORDS\n");
            }

            if (term(TokenType.TOKENS, false))
            {
                result = result && term(TokenType.TOKENS, true);
                while (term(TokenType.Ident, false))
                {
                    LexerToken tmpToken = new LexerToken();
                    result = result && TokenDecl(out tmpToken);
                    tokenDefinitions.Add(tmpToken);
                }

                form.appendText("parsed TOKENS\n");
            }

            while (term(TokenType.IGNORE, false))
            {
                result = result && WhiteSpaceDecl();
            }
            if (result)
            {
                form.appendText("Parsed scannerspec \n");
            }


            return result;
        }

        bool ParserSpecification()
        {
            bool result = true;
            if (term(TokenType.PRODUCTIONS, false))
            {
                result = result && term(TokenType.PRODUCTIONS, true);
                while (term(TokenType.Ident, false))
                {
                    ProductionClass tmpProduction = new ProductionClass();
                    result = result && Production(out tmpProduction);
                    parserDefinitions.Add(tmpProduction);
                }
                form.appendText("parsed KEYWORDS\n");
            }

            return term(TokenType.PRODUCTIONS, true);
        }
        bool Production(out ProductionClass tmpProduction)
        {
            tmpProduction = new ProductionClass();
            bool result = true;
            List<SymbolClass> expression = new List<SymbolClass>();
            string id = "";
            
            if (term(TokenType.Ident, false))
            {
                id = tokenEnumerator.Current.getValue();
                result = result && term(TokenType.Ident, true);
            }
            else {
                result = false;
            }

            if (term(TokenType.Attributes, false))
            {
                result = result && term(TokenType.Attributes, true);
            }

            if (term(TokenType.SemAction, false))
            {
                result = result && term(TokenType.SemAction, true);
            }

            if (term(TokenType.EQUALS, false))
            {
                result = result && term(TokenType.EQUALS, true)
                    && Expression(out expression);
            }
            else
                result = false;

            tmpProduction = new ProductionClass(id, expression);

            result = result && term(TokenType.DOT, true);
            if (result)
            {
                form.appendText("Parsed Tokendecl \n");
            }
            return result;
        }

        bool Expression(out List<SymbolClass> expression)
        {
            expression = new List<SymbolClass>();
            bool result = true;
            result = result && TokenTerm(out regExp);
            while (term(TokenType.OR, false))
            {
                string innerRegExp = "";
                regExp = regExp + LexerGenerationHelper.getEquivalent('|');
                result = result && term(TokenType.OR, true)
                    && TokenTerm(out innerRegExp);
                regExp = regExp + innerRegExp;
            }
            if (result)
            {
                form.appendText("Parsed TokenExpr \n");
            }
            return result;
        }

        bool Term(out string regExp)
        {
            regExp = "";
            bool result = true;
            result = result && TokenFactor(out regExp);

            while (term(TokenType.Ident, false) || term(TokenType.String, false) || term(TokenType.Char, false)
                || term(TokenType.LEFTPAR, false) || term(TokenType.LEFTSQUARE, false) || term(TokenType.LEFTCURLY, false))
            {
                string innerRegExp = "";
                result = result && TokenFactor(out innerRegExp);
                regExp = regExp + innerRegExp;
            }
            if (result)
            {
                form.appendText("Parsed TokenTerm \n");
            }
            return result;
        }

        bool Factor(out string regExp)
        {
            regExp = "";
            bool result = true;
            if (term(TokenType.Ident, false) || term(TokenType.String, false) || term(TokenType.Char, false)
                    || term(TokenType.LEFTPAR, false) || term(TokenType.LEFTSQUARE, false) || term(TokenType.LEFTCURLY, false))
            {
                do
                {
                    if (term(TokenType.Ident, false) || term(TokenType.String, false) || term(TokenType.Char, false))
                    {
                        result = result && Symbol(out regExp);
                        return result;
                    }

                    if (term(TokenType.LEFTPAR, false))
                    {
                        string innerRegexp = "";
                        result = result && term(TokenType.LEFTPAR, true)
                            && TokenExpr(out innerRegexp)
                            && term(TokenType.RIGHTPAR, true);
                        regExp = LexerGenerationHelper.getEquivalent('(') + innerRegexp + LexerGenerationHelper.getEquivalent(')');
                        return result;
                    }

                    if (term(TokenType.LEFTSQUARE, false))
                    {
                        string innerRegexp = "";
                        result = result && term(TokenType.LEFTSQUARE, true)
                            && TokenExpr(out innerRegexp)
                            && term(TokenType.RIGHTSQUARE, true);
                        regExp = LexerGenerationHelper.getEquivalent('(') + LexerGenerationHelper.getEquivalent('(')
                            + innerRegexp
                            + LexerGenerationHelper.getEquivalent(')') + LexerGenerationHelper.getEquivalent('|')
                            + LexerGenerationHelper.getEquivalent('3') + LexerGenerationHelper.getEquivalent(')');
                        return result;
                    }

                    if (term(TokenType.LEFTCURLY, false))
                    {
                        string innerRegexp = "";
                        result = result && term(TokenType.LEFTCURLY, true)
                            && TokenExpr(out innerRegexp)
                            && term(TokenType.RIGHTCURLY, true);

                        regExp = LexerGenerationHelper.getEquivalent('(')
                            + innerRegexp
                            + LexerGenerationHelper.getEquivalent(')') + LexerGenerationHelper.getEquivalent('*');
                        return result;
                    }
                }
                while (term(TokenType.Ident, false) || term(TokenType.String, false) || term(TokenType.Char, false)
                    || term(TokenType.LEFTPAR, false) || term(TokenType.LEFTSQUARE, false) || term(TokenType.LEFTCURLY, false));
            }
            else
            {
                return false;
            }
            if (result)
            {
                form.appendText("Parsed tokenfactor \n");
            }
            return result;
        }


        bool SetDecl(out LexerCharacterSet tmpToken)
        {
            bool result = true;
            string tempId;
            HashSet<char> tmp = new HashSet<char>();
            if (term(TokenType.Ident, false))
            {
                tempId = tokenEnumerator.Current.getValue();
                result = result && term(TokenType.Ident, true);
            }
            else tempId = "";
            result = result && term(TokenType.EQUALS, true);

            result = result && Set(out tmp);
            result = result && term(TokenType.DOT, true);

            tmpToken = new LexerCharacterSet(tempId, LexerGenerationHelper.getSetRegExp(tmp), tmp);

            return result;
        }

        bool Set(out HashSet<char> tmpSet)
        {
            tmpSet = new HashSet<char>();
            bool result = true;
            result = result && BasicSet(out tmpSet);
            while (term(TokenType.PLUS, false) || term(TokenType.MINUS, false))
            {
                if (term(TokenType.PLUS, false))
                {
                    HashSet<char> plusSet = new HashSet<char>();
                    result = result && term(TokenType.PLUS, true) && BasicSet(out plusSet);
                    tmpSet.UnionWith(plusSet);
                }
                if (term(TokenType.MINUS, false))
                {
                    HashSet<char> minusSet = new HashSet<char>();
                    result = result && term(TokenType.MINUS, true) && BasicSet(out minusSet);
                    tmpSet.ExceptWith(minusSet);
                }
            }

            if (result)
            {
                form.appendText("Parsed set \n");
            }
            return result;
        }

        bool BasicSet(out HashSet<char> tmpSet)
        {
            tmpSet = new HashSet<char>();
            bool result = true;
            if (term(TokenType.String, false) || term(TokenType.Ident, false) || term(TokenType.Char, false) || term(TokenType.CHR, false))
            {
                string currentString = tokenEnumerator.Current.getValue();
                if (term(TokenType.String, false))
                {
                    currentString = LexerGenerationHelper.getStringValue(currentString);
                    tmpSet.UnionWith(LexerGenerationHelper.getSet(currentString));
                    result = result && term(TokenType.String, true);
                    return result;
                }
                if (term(TokenType.Ident, false))
                {
                    foreach (LexerCharacterSet t in characterDefinitions)
                    {
                        if (currentString == t.Identifier)
                        {
                            tmpSet.UnionWith(t.PointedSet);
                        }
                    }
                    result = result && term(TokenType.Ident, true);

                    return result;
                }
                if (term(TokenType.Char, false) || term(TokenType.CHR, false))
                {
                    char tmpChar = ' ';
                    result = result && Char(out tmpChar);
                    if (term(TokenType.POINTPOINT, false))
                    {
                        char inferiorLimit = ' ';
                        result = result && term(TokenType.POINTPOINT, true)
                            && Char(out inferiorLimit);
                        tmpSet.UnionWith(LexerGenerationHelper.getRangeSet(tmpChar, inferiorLimit));
                        return result;
                    }
                    tmpSet.Add(tmpChar);
                    return result;
                }
            }
            else
            {
                return false;
            }

            if (result)
            {
                form.appendText("Parsed BasicSet \n");
            }
            return result;
        }

        bool Char(out char tmpChar)
        {
            tmpChar = ' ';
            bool result = true;
            if (term(TokenType.Char, false) || term(TokenType.CHR, false))
            {
                if (term(TokenType.Char, false))
                {
                    tmpChar = LexerGenerationHelper.getCharValue(tokenEnumerator.Current.getValue());
                    result = result && term(TokenType.Char, true);
                    return result;
                }
                if (term(TokenType.CHR, false))
                {
                    result = result && term(TokenType.CHR, true)
                        && term(TokenType.LEFTPAR, true);
                    if (term(TokenType.Number, false))
                    {
                        tmpChar = (char)Convert.ToInt32(tokenEnumerator.Current.getValue());
                        result = result && term(TokenType.Number, true)
                            && term(TokenType.RIGHTPAR, true);
                        return result;
                    }
                    else
                        return false;
                }
            }
            else
            {
                return false;
            }
            if (result)
            {
                form.appendText("Parsed Char \n");
            }
            return result;
        }

        bool KeywordDecl()
        {
            bool result = true;
            string id;
            string regexp;
            if (term(TokenType.Ident, false))
            {
                id = tokenEnumerator.Current.getValue();
                result = result && term(TokenType.Ident, true);
            }
            else
            {
                return false;
            }
            result = result && term(TokenType.EQUALS, true);
            if (term(TokenType.String, false))
            {
                regexp = LexerGenerationHelper.getStringValue(tokenEnumerator.Current.getValue());
                result = result && term(TokenType.String, true);
            }
            else
            {
                return false;
            }
            result = result && term(TokenType.DOT, true);
            LexerKeyword tempToken = new LexerKeyword(id, regexp);
            keywordDefinitions.Add(tempToken);
            return result;
        }

        bool TokenDecl(out LexerToken tmpToken)
        {
            tmpToken = new LexerToken();
            bool result = true;
            string regexp = "";
            string id = "";
            if (term(TokenType.Ident, false))
            {
                id = tokenEnumerator.Current.getValue();
                result = result && term(TokenType.Ident, true);
            }
            if (term(TokenType.EQUALS, false))
            {
                result = result && term(TokenType.EQUALS, true)
                    && TokenExpr(out regexp);
            }

            if (term(TokenType.EXCEPT, false))
            {
                result = result && term(TokenType.EXCEPT, true)
                    && term(TokenType.KEYWORDS, true);
            }

            tmpToken = new LexerToken(id, regexp);

            result = result && term(TokenType.DOT, true);
            if (result)
            {
                form.appendText("Parsed Tokendecl \n");
            }
            return result;
        }

        bool TokenExpr(out string regExp)
        {
            regExp = "";
            bool result = true;
            result = result && TokenTerm(out regExp);
            while (term(TokenType.OR, false))
            {
                string innerRegExp = "";
                regExp = regExp + LexerGenerationHelper.getEquivalent('|');
                result = result && term(TokenType.OR, true)
                    && TokenTerm(out innerRegExp);
                regExp = regExp + innerRegExp;
            }
            if (result)
            {
                form.appendText("Parsed TokenExpr \n");
            }
            return result;
        }

        bool TokenTerm(out string regExp)
        {
            regExp = "";
            bool result = true;
            result = result && TokenFactor(out regExp);

            while (term(TokenType.Ident, false) || term(TokenType.String, false) || term(TokenType.Char, false)
                || term(TokenType.LEFTPAR, false) || term(TokenType.LEFTSQUARE, false) || term(TokenType.LEFTCURLY, false))
            {
                string innerRegExp = "";
                result = result && TokenFactor(out innerRegExp);
                regExp = regExp + innerRegExp;
            }
            if (result)
            {
                form.appendText("Parsed TokenTerm \n");
            }
            return result;
        }

        bool TokenFactor(out string regExp)
        {
            regExp = "";
            bool result = true;
            if (term(TokenType.Ident, false) || term(TokenType.String, false) || term(TokenType.Char, false)
                    || term(TokenType.LEFTPAR, false) || term(TokenType.LEFTSQUARE, false) || term(TokenType.LEFTCURLY, false))
            {
                do
                {
                    if (term(TokenType.Ident, false) || term(TokenType.String, false) || term(TokenType.Char, false))
                    {
                        result = result && Symbol(out regExp);
                        return result;
                    }

                    if (term(TokenType.LEFTPAR, false))
                    {
                        string innerRegexp = "";
                        result = result && term(TokenType.LEFTPAR, true)
                            && TokenExpr(out innerRegexp)
                            && term(TokenType.RIGHTPAR, true);
                        regExp = LexerGenerationHelper.getEquivalent('(') + innerRegexp + LexerGenerationHelper.getEquivalent(')');
                        return result;
                    }

                    if (term(TokenType.LEFTSQUARE, false))
                    {
                        string innerRegexp = "";
                        result = result && term(TokenType.LEFTSQUARE, true)
                            && TokenExpr(out innerRegexp)
                            && term(TokenType.RIGHTSQUARE, true);
                        regExp = LexerGenerationHelper.getEquivalent('(') + LexerGenerationHelper.getEquivalent('(')
                            + innerRegexp
                            + LexerGenerationHelper.getEquivalent(')') + LexerGenerationHelper.getEquivalent('|')
                            + LexerGenerationHelper.getEquivalent('3') + LexerGenerationHelper.getEquivalent(')');
                        return result;
                    }

                    if (term(TokenType.LEFTCURLY, false))
                    {
                        string innerRegexp = "";
                        result = result && term(TokenType.LEFTCURLY, true)
                            && TokenExpr(out innerRegexp)
                            && term(TokenType.RIGHTCURLY, true);

                        regExp = LexerGenerationHelper.getEquivalent('(')
                            + innerRegexp
                            + LexerGenerationHelper.getEquivalent(')') + LexerGenerationHelper.getEquivalent('*');
                        return result;
                    }
                }
                while (term(TokenType.Ident, false) || term(TokenType.String, false) || term(TokenType.Char, false)
                    || term(TokenType.LEFTPAR, false) || term(TokenType.LEFTSQUARE, false) || term(TokenType.LEFTCURLY, false));
            }
            else
            {
                return false;
            }
            if (result)
            {
                form.appendText("Parsed tokenfactor \n");
            }
            return result;
        }

        bool Symbol(out string regexp)
        {
            regexp = "";
            bool result = true;
            if (term(TokenType.Ident, false) || term(TokenType.String, false) || term(TokenType.Char, false))
            {
                string currentString = tokenEnumerator.Current.getValue();
                if (term(TokenType.Ident, false))
                {
                    foreach (LexerCharacterSet t in characterDefinitions)
                    {
                        if (currentString == t.Identifier)
                        {
                            regexp = t.Regexp;
                        }
                    }
                    result = result && term(TokenType.Ident, true);
                }

                if (term(TokenType.String, false))
                {
                    string tmpString = LexerGenerationHelper.getStringValue(currentString);
                    regexp = tmpString;

                    result = result && term(TokenType.String, true);
                }

                if (term(TokenType.Char, false))
                {
                    regexp = LexerGenerationHelper.getCharValue(currentString).ToString();
                    result = result && term(TokenType.Char, true);
                }
            }
            else
            {
                return false;
            }
            if (result)
            {
                form.appendText("Parsed symbol \n");
            }
            return result;
        }
        
        bool WhiteSpaceDecl()
        {
            HashSet<char> whiteSet = new HashSet<char>();
            bool result = term(TokenType.IGNORE, true) && Set(out whiteSet);;
            whitespaceDefinitions = new LexerCharacterSet("whitespace", LexerGenerationHelper.getSetRegExp(whiteSet), whiteSet);
            return result;
        }
    }
}
