using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace Parser
{
    public partial class MainForm : Form
    {
        List<LexerCharacterSet> characterDefinitions = new List<LexerCharacterSet>();
        List<LexerKeyword> keywordDefinitions = new List<LexerKeyword>();
        List<LexerToken> tokenDefinitions = new List<LexerToken>();
        LexerCharacterSet whitespaceDefinitions = new LexerCharacterSet();

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the automaton real character
        /// </summary>
        /// <param name="c">The character to convert</param>
        /// <returns>A string with the automaton real character</returns>
       

        private void BrowseBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            stringViewer.Visible = false;
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                Tokenizer tk = new Tokenizer(OFD.FileName, this);
            }
            GenerateBtn.Enabled = true;
            stringViewer.Visible = true;
        }

        public void setLexer(List<LexerCharacterSet> thecharacterDefinitions, List<LexerKeyword> thekeywordDefinitions,
            List<LexerToken> thetokenDefinitions, LexerCharacterSet thewhitespaceDefinitions)
        {
            characterDefinitions = thecharacterDefinitions;
            keywordDefinitions = thekeywordDefinitions;
            tokenDefinitions = thetokenDefinitions;
            whitespaceDefinitions = thewhitespaceDefinitions;
        }


        void generateFile(string FileName)
        {

            String file;
            using (StreamReader myStream = new StreamReader("Program.frame"))
            {
                file = myStream.ReadToEnd();
                string enumDefinition = "";
                string tokenDefinitionString = "";
                string whiteDefinitionString = "";
                List<string> theMembers = new List<string>();
                foreach (LexerKeyword lk in keywordDefinitions)
                {
                    theMembers.Add(lk.Identifier.ToUpper());
                    tokenDefinitionString += "theTokensDefinition.Add(new TokenDefinition(TokenType."+lk.Identifier.ToUpper()+", \""+lk.Regexp+"\")); \n";
                }
                foreach (LexerToken lk in tokenDefinitions)
                {
                    theMembers.Add(lk.Identifier.ToUpper());
                    tokenDefinitionString += "theTokensDefinition.Add(new TokenDefinition(TokenType." + lk.Identifier.ToUpper() + ", \"" + lk.Regexp + "\")); \n";
                }
                if (whitespaceDefinitions.PointedSet!=null)
                foreach (char lk in whitespaceDefinitions.PointedSet)
                {
                    whiteDefinitionString += "file = file.Replace(\""+lk+"\", \"\"); \n";
                }
                foreach (string s in theMembers)
                {
                    if (s != theMembers[theMembers.Count - 1])
                        enumDefinition += s + ", \n";
                    else {
                        enumDefinition += s + "\n";
                    }
                }
                file = file.Replace(@"/*replacementDEF*/", enumDefinition);
                file = file.Replace(@"/*replacementTOKEN*/", tokenDefinitionString);
                file = file.Replace(@"/*replacementWhitespace*/", whiteDefinitionString);  
            }
            File.WriteAllText(FileName, file);
        }

        private void GenerateBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Filter = "cSharp files (*.cs)|*.cs";

            if (SFD.ShowDialog() == DialogResult.OK)
            {
                generateFile(SFD.FileName);
            }
            MessageBox.Show("DONE!");
        }
        public void appendText(string theString)
        {
            stringViewer.AppendText(theString);
        }
    }
}
