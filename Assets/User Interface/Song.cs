using System.Collections;
using System.Collections.Generic;

public class Song
{
    private string name = "";
    private int count = 0;
    private List<string> lyrics = new List<string>();

    public Song(string givenName, List<string> givenLyrics)
    {
        name = givenName;
        lyrics = givenLyrics;
        count = 0;
    }

    public string getName() { return name; }
    public string nextVerse( int marchCount)
    {
        if (marchCount % 2 != 0 && count == 0) { return ""; } // Don't start on right leg
        return lyrics[count++];
    }
    public bool ended()
    {
        return count >= getSize();
    }
    public void resetCount() { count = 0; }


    public void addVerse(string verse) {
        lyrics.Add(verse);
    }

    public string getVerse(int versePos)
    {
        return lyrics[versePos];
    }
    public int getSize()
    {
        return lyrics.Count;
    }
}
