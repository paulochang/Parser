using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Text;

namespace Parser
{
    public enum TokenType
    {
        COMPILER,
        END,
        DOT,
        POINTPOINT,
        OR,
        LEFTPAR,
        RIGHTPAR,
        LEFTSQUARE,
        RIGHTSQUARE,
        LEFTCURLY,
        RIGHTCURLY,
        CHARACTERS,
        KEYWORDS,
        TOKENS,
        CHR,
        EXCEPT,
        IGNORE,
        PRODUCTIONS,
        EQUALS,
        PLUS,
        MINUS,
        Attributes,
        SemAction,
        Ident,
        Number,
        String,
        Char
    }

    class Tokenizer
    {

        const int Accepted = 1;
        const int notAccepted = 0;
        const int inProcess = -1;

        List<TokenDefinition> theTokensDefinition = new List<TokenDefinition>();
        List<Token> theTokenList;

                public Tokenizer(string FileName, MainForm form)
        {
            string letters = LexerGenerationHelper.getBigOr(new List<char>{'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'});
            string digits = LexerGenerationHelper.getBigOr(new List<char>{'1','2','3','4','5','6','7','8','9','0'});
            string anybutquote = LexerGenerationHelper.getRegExpExcept(34);
            string anybutapostrophe = LexerGenerationHelper.getRegExpExcept(39);
            string any = LexerGenerationHelper.getAny();
            theTokensDefinition.Add(new TokenDefinition(TokenType.POINTPOINT, ".."));
            theTokensDefinition.Add(new TokenDefinition(TokenType.OR, "|"));
            theTokensDefinition.Add(new TokenDefinition(TokenType.DOT, "."));
            theTokensDefinition.Add(new TokenDefinition(TokenType.LEFTPAR, "("));
            theTokensDefinition.Add(new TokenDefinition(TokenType.RIGHTPAR, ")"));
            theTokensDefinition.Add(new TokenDefinition(TokenType.LEFTSQUARE, "["));
            theTokensDefinition.Add(new TokenDefinition(TokenType.RIGHTSQUARE, "]"));
            theTokensDefinition.Add(new TokenDefinition(TokenType.LEFTCURLY, "{"));
            theTokensDefinition.Add(new TokenDefinition(TokenType.RIGHTCURLY, "}"));
            theTokensDefinition.Add(new TokenDefinition(TokenType.PLUS, "+"));
            theTokensDefinition.Add(new TokenDefinition(TokenType.MINUS, "-"));
            theTokensDefinition.Add(new TokenDefinition(TokenType.CHR, "CHR"));
            theTokensDefinition.Add(new TokenDefinition(TokenType.END, "END"));
            theTokensDefinition.Add(new TokenDefinition(TokenType.EQUALS, "="));
            theTokensDefinition.Add(new TokenDefinition(TokenType.TOKENS, "TOKENS"));
            theTokensDefinition.Add(new TokenDefinition(TokenType.EXCEPT, "EXCEPT"));
            theTokensDefinition.Add(new TokenDefinition(TokenType.IGNORE, "IGNORE"));
            theTokensDefinition.Add(new TokenDefinition(TokenType.KEYWORDS, "KEYWORDS"));
            theTokensDefinition.Add(new TokenDefinition(TokenType.COMPILER, "COMPILER"));
            theTokensDefinition.Add(new TokenDefinition(TokenType.CHARACTERS,"CHARACTERS"));
            theTokensDefinition.Add(new TokenDefinition(TokenType.PRODUCTIONS,"PRODUCTIONS"));
            theTokensDefinition.Add(new TokenDefinition(TokenType.Attributes, "<." + any + LexerGenerationHelper.getEquivalent('*') + ".>"));
            theTokensDefinition.Add(new TokenDefinition(TokenType.SemAction, "(." + any + ".)"));
            theTokensDefinition.Add(new TokenDefinition(TokenType.Ident, letters + LexerGenerationHelper.getEquivalent('(') + letters + LexerGenerationHelper.getEquivalent('|') + digits + LexerGenerationHelper.getEquivalent(')') + LexerGenerationHelper.getEquivalent('*')));
            theTokensDefinition.Add(new TokenDefinition(TokenType.Number, digits + digits + LexerGenerationHelper.getEquivalent('*')));
            theTokensDefinition.Add(new TokenDefinition(TokenType.String, "\"" + anybutquote + LexerGenerationHelper.getEquivalent('*') + "\""));
            theTokensDefinition.Add(new TokenDefinition(TokenType.Char,"'"+anybutapostrophe+"'"));
             
            try
            {
                using (StreamReader myStream = new StreamReader(FileName))
                {
                    String file;
                    file = myStream.ReadToEnd();
                    theTokenList = getTokens(file, form);
                    foreach (Token t in theTokenList)
                    {
                        form.appendText(t.getValue() + " = " + t.getTokenType()+"\n");
                    }
                }
                Parser ps = new Parser(theTokenList);
                ps.Parse(form);
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error reading the file: " + ex.Message);
            }
        }

        private List<Token> getTokens(string file, MainForm theForm)
        {
            List<Token> result = new List<Token>();
            string[] fileStrings = file.Split(new char[] { '\r', '\n', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                foreach (string s in fileStrings)
                {
                    string tempString = s;
                    int guessedChars = s.Length;
                    do
                    {
                        bool foundsomething = false;
                        foreach (TokenDefinition t in theTokensDefinition)
                        {
                            if (t.evaluate(tempString.Substring(0, guessedChars)))
                            {
                                foundsomething = true;
                                result.Add(new Token(t.getTokenType(), tempString.Substring(0, guessedChars)));
                                tempString = tempString.Remove(0, guessedChars);
                                guessedChars = tempString.Length;
                                break;
                            }
                        }
                        if (!foundsomething) guessedChars--;
                    } while ((tempString != "") || guessedChars > 0);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("There was an error parsing the file "+ex.Message);
            }
            return result;
        } //getTokens-END

        public List<Token> getTokenList()
        {
            return theTokenList;
        }

    }// class-END
}//namespace-END
