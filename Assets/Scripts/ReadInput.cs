using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Voxell;
using Voxell.NLP.Parser;
using UnityEngine;

public class ReadInput : MonoBehaviour
{
    public TreeParent tree;
    string parserModel = Application.streamingAssetsPath + "/Models/";
    private EnglishTreebankParser parser;

    void Start()
    {
        parser = new EnglishTreebankParser(FileUtilx.GetStreamingAssetFilePath(parserModel));
    }

    public void ReadsInput(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            tree.parse = null;
            tree.DeleteTree();
        }
        else
        {
            Parse[] par = DoParsing(s);
            int offset = 0;

            foreach (Parse p in par)
            {
                tree.Recurse(p, offset);
                offset++;
            }
        }
    }

    Parse[] DoParsing(string s)
    {
        string[] lines = DetectSentence(s);
        Parse[] par = parser.DoParse(lines);
        return par;
    }

    string[] DetectSentence(string s)
    {
        string[] sentences = Regex.Split(s, @"(?<=[\.!\?])\s+");
        return sentences;
    }

    void RoutePOS(List<KeyValuePair<string, string>> tags)
    {
        foreach(var kv in tags)
        {
            string val = kv.Value;
            string valTop = val.Substring(0,2);
            print(valTop);

            /****** ADJECTIVES & CARDINAL NUMBERS ******/
            /*
                CD	Cardinal number
                JJ	Adjective
                JJR	Adjective, comparative
                JJS	Adjective, superlative
            */
            if (string.Equals(val, "CD") || string.Equals(valTop, "JJ"))
            {
                print("Number or adjective");
            }

            /****** ADVERBS ******/
            /*
                RB	Adverb
                RBR	Adverb, comparative
                RBS	Adverb, superlative
            */
            else if (string.Equals(valTop, "RB"))
            {
                print("Adverb");
            }
    
            /****** NOUNS ******/
            /*
                NN	Noun, singular or mass
                NNS	Noun, plural
                NNP	Proper noun, singular
                NNPS Proper noun, plural
                PRP	Personal pronoun
            */
            else if (string.Equals(valTop, "NN") || string.Equals(val, "PRP"))
            {
                print("Noun or pronoun");
            }

            /****** PREPOSITIONS ******/
            /*
                IN	Preposition or subordinating conjunction
            */
            else if (string.Equals(val, "IN"))
            {
                print("Preposition");
            }

            /****** VERBS ******/
            /*
                VB	Verb, base form
                VBD	Verb, past tense
                VBG	Verb, gerund or present participle
                VBN	Verb, past participle
                VBP	Verb, non-3rd person singular present
                VBZ	Verb, 3rd person singular present
            */
            else if (string.Equals(valTop, "VB"))
            {
                print("Verb");
            }

            /****** TBD ******/
            /*
            CC	Coordinating conjunction
            DT	Determiner
            EX	Existential there
            FW	Foreign word
            LS	List item marker
            MD	Modal
            PDT	Predeterminer
            POS	Possessive ending
            PRP$ Possessive pronoun
            RP	Particle
            SYM	Symbol
            TO	to
            UH	Interjection
            WDT	Wh-determiner
            WP	Wh-pronoun
            WP$	Possessive wh-pronoun
            WRB	Wh-adverb
            */
        }
    }
}