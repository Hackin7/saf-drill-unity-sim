using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class File : MonoBehaviour
{

    // Data
    private List<GameObject> men = new List<GameObject>();
    public GameObject manSample;
    public float socialDistancing = 1.0f;

    private int count = 0; //  Easier to count manually

    public bool silentDrill = false;
    private int direction = 0; // 0 for forward, 1 for right, 2.....
    private int state = 0; // 0 for stationary, 1 for moving, 2 for march/ hentak/ run, 3 for alignment
    private string subtitle = "";

    private float speed = 1.0f;


    // Handling Variables
    public int getState(){return state; }
    public bool allSedia()
    {
        foreach (GameObject man in men) {
            if (man.GetComponent<Man>().getState() != 1) {
                return false;
            };
        }
        return true;
    }
    public string getSubtitles()
    {
        return subtitle;
    }

    // Fall In ////////////////////////////////////////////////////
    private int fileDepth(int count)
    {
        switch (count % 3) {
            case 0: return 0;
            case 1: return 2;
            case 2: return 1;
            default: return 0;
        }
    }
    public void addMan()
    {
        // initialse gameobject 
        Vector3 offsetPos = new Vector3(socialDistancing * (count / 3), 0f, socialDistancing * (count % 3));
        
        GameObject newMan = Instantiate(manSample);
        newMan.transform.parent = transform;


        if (count < 3)
        {
            offsetPos = new Vector3(0f, 0f, socialDistancing * (count % 3));
            men.Add(newMan);
        }
        else if (count < 6)
        {
            offsetPos = new Vector3(socialDistancing, 0f, socialDistancing * fileDepth(count));
            men.Add(newMan);
        }
        // Normal Scenario
        else
        {
            if (count % 3 == 0) //  Shift Out last 3 men
            {
                for (int i=count - 3; i < count; i++)
                {
                    men[i].transform.Translate(new Vector3(socialDistancing, 0f, 0f));
                }
            }

            men.Insert(count - 3, newMan);
            offsetPos = new Vector3(socialDistancing * (count / 3 - 1), 0f, socialDistancing * fileDepth(count));
        }

        newMan.transform.position = offsetPos + transform.position;
        // Rename man for debugging purposes
        if (true)//Application.isEditor && !Application.isPlaying)
        {
            for (int i = 0; i < getManCount(); i++)
            {
                men[i].name = manSample.name + " " + (i).ToString();
            }
        }
        //newMan.name = manSample.name + " " + getManCount().ToString();
        count++;
    }
    public void clearMen()
    {
        count = 0;
        foreach (GameObject man in men)
        {
            Destroy(man);
        }
        men = new List<GameObject>();
    }
    public void setManCount(int count)
    {
        clearMen();
        for (int i = 0; i < count; i++)
        {
            addMan();
        }
        // Reposition file to be in middle
        transform.position = new Vector3(-(count / 3 * socialDistancing) / 2, transform.position.y, transform.position.z);
    }
    public int getManCount()
    {
        return count;
    }
    
    /// Timing //////////////////////////
    IEnumerator waitToFinish()
    {
        bool finished = false;
        while (!finished)
        {
            finished = true;
            foreach (GameObject man in men)
            {
                if (!man.GetComponent<Man>().isFinishedMotion())
                {
                    finished = false;
                }
            }
            yield return new WaitForSeconds(0.1f); //Haha imagine using proper event handlers
        }
    }

    public void setSpeed(float givenSpeed)
    {
        speed = givenSpeed;
        foreach (GameObject man in men)
        {
            try
            {
                if (man != null)
                {
                    man.GetComponent<Man>().setSpeed(givenSpeed);
                }
            }catch (System.NullReferenceException)
            { }
            
        }
    }
    public float getSpeed() { return speed; }
    // Commands ///////////////////////////////////////////////////
    IEnumerator sedia()
    {
        state = 1;
        if (men[0].GetComponent<Man>().getState() == 2) { subtitle = "down"; }
        else { subtitle = "check"; }
        foreach (GameObject man in men) { man.GetComponent<Man>().lift_left_leg(); }
        yield return waitToFinish();
        
        subtitle = "";
        foreach (GameObject man in men) { man.GetComponent<Man>().stomp(); man.GetComponent<Man>().sedia(); }
        state = 0;
    }
    IEnumerator senangdiri()
    {
        state = 1;
        subtitle = "check";
        foreach (GameObject man in men) { man.GetComponent<Man>().lift_left_leg(); }
        yield return waitToFinish();

        subtitle = "";
        foreach (GameObject man in men) { man.GetComponent<Man>().senangdiri(); }
        state = 0;
        yield return waitToFinish();
    }
    IEnumerator turn_right()
    {
        state = 1;
        direction++; if(direction > 4) { direction -= 4; }
        subtitle = "turn";
        foreach (GameObject man in men) { man.GetComponent<Man>().turn_right(); }
        yield return waitToFinish();
        subtitle = "check";
        foreach (GameObject man in men) { man.GetComponent<Man>().lift_left_leg(); }
        yield return waitToFinish();
        subtitle = "";
        foreach (GameObject man in men) { man.GetComponent<Man>().stomp(); }
        state = 0;
        yield return waitToFinish();
    }
    IEnumerator turn_left()
    {
        state = 1;
        direction++; if (direction > 4) { direction -= 4; }
        subtitle = "turn";
        foreach (GameObject man in men) { man.GetComponent<Man>().turn_left(); }
        yield return waitToFinish();
        subtitle = "check";
        foreach (GameObject man in men) { man.GetComponent<Man>().lift_right_leg(); }
        yield return waitToFinish();
        subtitle = "";
        foreach (GameObject man in men) { man.GetComponent<Man>().stomp(); }
        state = 0;
        yield return waitToFinish();
    }
    IEnumerator turn_back()
    {
        state = 1;
        direction+=2; if (direction > 4) { direction -= 4; }
        Debug.Log(direction);
        subtitle = "turn";
        foreach (GameObject man in men) { man.GetComponent<Man>().turn_back(); }
        yield return waitToFinish();
        subtitle = "check";
        foreach (GameObject man in men) { man.GetComponent<Man>().lift_right_leg(); }
        yield return waitToFinish();
        subtitle = "";
        foreach (GameObject man in men) { man.GetComponent<Man>().stomp(); }
        state = 0;
        yield return waitToFinish();
    }

    IEnumerator alignRight()
    {
        state = 1;
        subtitle = "up";
        for (int i=3; i<getManCount(); i++) { men[i].GetComponent<Man>().headUpTilt(); }
        yield return waitToFinish();

        subtitle = "check";
        for (int i = 1; i < getManCount(); i++) { men[i].GetComponent<Man>().lift_right_leg(); }
        yield return waitToFinish();
        subtitle = "";
        for (int i = 1; i < getManCount(); i++) { men[i].GetComponent<Man>().stomp(); }
        yield return waitToFinish();

        string[] stuff = { "5", "4", "3", "2", "1", "All adjustments", "cut" };
        for (int i=0; i < 7; i++)
        {
            subtitle = stuff[i];
            yield return new WaitForSeconds(1.0f / speed);
        }
        subtitle = "";
        state = 3;
    }
    IEnumerator headNormalTilt()
    {
        state = 1;
        subtitle = "down";
        foreach (GameObject man in men) { man.GetComponent<Man>().headNormalTilt(); }
        yield return waitToFinish();
        subtitle = "";
        state = 0;
    }
    IEnumerator openOrder()
    {
        state = 1;
        foreach (int manCounter in frontRowIndexes(true)) { men[manCounter].GetComponent<Man>().stepForward(socialDistancing); }
        foreach (int manCounter in backRowIndexes(true)) { men[manCounter].GetComponent<Man>().stepBack(socialDistancing); }
        yield return waitToFinish();
        subtitle = "check";
        for (int i = 1; i < getManCount(); i++) { men[i].GetComponent<Man>().lift_right_leg(); }
        yield return waitToFinish();
        subtitle = "";
        for (int i = 1; i < getManCount(); i++) { men[i].GetComponent<Man>().stomp(); }
        state = 0;
    }
    IEnumerator closeOrder()
    {
        state = 1;
        foreach (int manCounter in frontRowIndexes(true)) { men[manCounter].GetComponent<Man>().stepBack(socialDistancing); }
        foreach (int manCounter in backRowIndexes(true)) { men[manCounter].GetComponent<Man>().stepForward(socialDistancing); }
        yield return waitToFinish();
        subtitle = "check";
        for (int i = 1; i < getManCount(); i++) { men[i].GetComponent<Man>().lift_right_leg(); }
        yield return waitToFinish();
        subtitle = "";
        for (int i = 1; i < getManCount(); i++) { men[i].GetComponent<Man>().stomp(); }
        state = 0;
    }

    private List<int> frontRowIndexes( bool includeLast )
    {
        List<int> indexes = new List<int>();
        int manCount = getManCount();
        int manCounter = 0;
        while (manCounter < manCount - 3)
        {
            indexes.Add(manCounter);
            manCounter += 3;
        }
        if (includeLast) { indexes.Add(manCount - 3); }
        return indexes;
    }
    private List<int> backRowIndexes(bool includeLast)
    {
        List<int> indexes = new List<int>();
        int manCount = getManCount();

        if (2 < manCount) { indexes.Add(2); }
        int manCounter = 4;
        while (manCounter < manCount - 3)
        {
            indexes.Add(manCounter);
            manCounter += 3;
        }
        if (includeLast) { indexes.Add(manCount - 2); }
        return indexes;
    }
    IEnumerator manSay(int manIndex, string speech)
    {
        men[manIndex].GetComponent<Man>().say(speech); subtitle = speech;
        yield return waitToFinish();
        subtitle = "";
    }
    IEnumerator bilang()
    {
        state = 1;
        if (count < 6)
        {

        }
        else
        {
            int manCount = getManCount();
            int label = 1;

            foreach(int manCounter in frontRowIndexes(false))
            {
                men[manCounter].GetComponent<Man>().setSpeed(speed / 1.5f);
                yield return manSay(manCounter, label++.ToString());
                men[manCounter].GetComponent<Man>().setSpeed(speed);
            }
            yield return manSay(manCount - 3, label.ToString() + " Last");

            yield return new WaitForSeconds(1.0f / speed);

            //Final Report
            men[manCount - 2].GetComponent<Man>().setSpeed(speed / 3);
            string[] report = { label.ToString() + " Rows", (manCount % 3 == 0 ? "No" : (3 - manCount % 3).ToString()) + " Blank Files", "No man behind IC" };
            for (int i = 0; i < 3; i++)
            {
                yield return manSay(manCount - 2, report[i]);
            }
            men[manCount - 2].GetComponent<Man>().setSpeed(speed);
        }
        yield return waitToFinish();
        state = 0;
    }
    // Marching //////////////////////////////////////////////////////
    private int moveState = 0; // march, hentak
    private int prevMoveState = 0;
    private int marchCount = 0;
    private Song march_tempo = new Song("tempo", new List<string>( new string[] { "left", "", "left", "", "left", "right", "left", "", "left", "", "left", "", "left", "right", "", "" }) );
    private Song march_song;

    // Controlling /////
    public void start_march()
    {
        moveState = 0;
        if (state != 2)
        {
            marchCount = 0;
            state = 2;

            StartCoroutine(march_cycle());
        }
    }
    public void stop_march()
    {
        state = 1;
    }
    public void tsukalanka()
    {
        state = 3;
    }
    public void start_hentak()
    {
        moveState = 1;
        if (state != 2)
        {
            marchCount = 0;
            state = 2;
            
            StartCoroutine(march_cycle());
        }
    }

    public void setSong(Song song){march_song = song;}
    // Actions /////
    private void march_left(){ foreach (GameObject man in men) { man.GetComponent<Man>().march_left(); } }
    private void march_right(){ foreach (GameObject man in men) { man.GetComponent<Man>().march_right(); } }

    private void hentak_left(){ foreach (GameObject man in men) { man.GetComponent<Man>().hentak_left(); } }
    private void hentak_right() { foreach (GameObject man in men) { man.GetComponent<Man>().hentak_right(); } }

    public bool isMarching()
    {
        return state == 2;
    }
    private void march_action()
    {
        if (moveState == 0)
        {
            if (marchCount % 2 == 0) { march_left(); }
            else { march_right(); }
        }
        else if (moveState == 1)
        {
            if (marchCount % 2 == 0) { hentak_left(); }
            else { hentak_right(); }
        }
        marchCount = (marchCount + 1) % 2;
    }

    // Cycle /////
    IEnumerator march_cycle()
    {
        while (state == 2)
        {
            subtitle = march_song.nextVerse(marchCount);
            if (march_song.ended()) { march_song = march_tempo; march_song.resetCount(); }
            
            march_action();
            
            yield return waitToFinish();
            prevMoveState = moveState;
            
        }
        subtitle = "";
        if (state == 1){StartCoroutine(reach_stop());}
        else if (state == 3) { StartCoroutine(syncTiming()); }
    }

    IEnumerator reach_stop()
    {
        string[] timer = { "check", "one" };
        int count = 0;
        // Align if needed
        if (moveState == 0)
        {
            if (marchCount % 2 == 1) { march_action(); subtitle = timer[count]; count++; }
        }
        else if (moveState == 1){
            if (marchCount % 2 == 0) { march_action(); subtitle = timer[count]; count++; }
        }
        
        yield return waitToFinish();
        march_action();
        yield return waitToFinish();
        subtitle = timer[count];
        foreach (GameObject man in men)
        {
            man.GetComponent<Man>().setSpeed(getSpeed() * 1.5f);
            if (moveState == 0) { man.GetComponent<Man>().lift_right_leg(); }
            else if (moveState == 1) { march_action(); }
        }
        yield return waitToFinish();

        subtitle = "";
        foreach (GameObject man in men) { man.GetComponent<Man>().stomp(); }
        yield return waitToFinish();
        foreach (GameObject man in men) { man.GetComponent<Man>().setSpeed(getSpeed()); }
        state = 0;
    }
    IEnumerator syncTiming()
    {
        state = 1;
        subtitle = "check";
        foreach (GameObject man in men)
        {
            man.GetComponent<Man>().setSpeed(getSpeed() * 2);
            if (marchCount % 2 == 1) { man.GetComponent<Man>().lift_right_leg(); }
            else { man.GetComponent<Man>().lift_left_leg(); }
        }
        yield return waitToFinish();

        subtitle = "";
        foreach (GameObject man in men) { man.GetComponent<Man>().stomp(); }
        yield return waitToFinish();
        foreach (GameObject man in men) { man.GetComponent<Man>().setSpeed(getSpeed()); }
        start_march();
    }

    IEnumerator tahat()
    {
        state = 1;
        foreach (GameObject man in men) { man.GetComponent<Man>().tahat(); }
        subtitle = "up";
        yield return waitToFinish();
        subtitle = "";
        state = 0;
    }
    IEnumerator hormat()
    {
        state = 1;
        foreach (GameObject man in men) { man.GetComponent<Man>().hormat(); }
        subtitle = "up";
        yield return waitToFinish();
        subtitle = "";
        state = 0;
    }
    IEnumerator keluarBaris()
    {
        state = 1;
        foreach (GameObject man in men) { man.GetComponent<Man>().turn_right(); }
        subtitle = "turn";
        yield return waitToFinish();

        foreach (GameObject man in men) { man.GetComponent<Man>().lift_left_leg(); }
        subtitle = "check";
        yield return waitToFinish();

        foreach (GameObject man in men) { man.GetComponent<Man>().stomp(); }
        subtitle = "";
        yield return waitToFinish();

        subtitle = "check";
        yield return new WaitForSeconds(0.25f);

        string[] cheer = { "Brother's", "First", "Always", "Bravo" };
        for (int i=0; i<3; i++)
        {
            if (i%2 == 0) { foreach (GameObject man in men) { man.GetComponent<Man>().march_left(); }}
            else { foreach (GameObject man in men) { man.GetComponent<Man>().march_right(); } }
            
            subtitle = cheer[i];
            yield return waitToFinish();
        }
        
        foreach (GameObject man in men) {
            man.GetComponent<Man>().setSpeed(getSpeed() * 2);
            man.GetComponent<Man>().lift_right_leg();
        }
        subtitle = "";
        yield return waitToFinish();

        foreach (GameObject man in men) { man.GetComponent<Man>().stomp(); }
        subtitle = "";
        yield return waitToFinish();

        foreach (GameObject man in men) {
            man.GetComponent<Man>().setSpeed(getSpeed());
            man.GetComponent<Man>().fist(); }
        subtitle = cheer[3];
        yield return waitToFinish();
        yield return new WaitForSeconds(1.0f/speed);

        subtitle = "(Falling Out)";
        int prevManCount = getManCount();
        setManCount(0);
        yield return new WaitForSeconds(1.0f/speed);

        subtitle = "";
        setManCount(prevManCount);
        state = 0;
    }

    // Command List /////////////////////////////////////////////////////
    public void giveCommand(string command)
    {   switch (command)
            {
                case "sedia": StartCoroutine(sedia()); break;
                case "senang diri": StartCoroutine(senangdiri()); break;

                case "ke kenan pu seng": StartCoroutine(turn_right()); break;
                case "ke kiri pu seng": StartCoroutine(turn_left()); break;
                case "ke belakang pu seng": StartCoroutine(turn_back()); break;

                case "dalam buka barisan": StartCoroutine(openOrder()); break;
                case "dalam tutup barisan": StartCoroutine(closeOrder());  break;
                case "ke kenan lu rus": StartCoroutine(alignRight()); break;
                case "pandang ke hadapan pandang": StartCoroutine(headNormalTilt()); break;
                case "maju":
                case "cepat jalan": start_march(); break;
                case "berhenti": stop_march(); break;
                case "tahat sedia": StartCoroutine(tahat());break;
                case "hormat": StartCoroutine(hormat()); break;
                case "dari kenan bilang":StartCoroutine(bilang());break;
                case "hentak kaki cepat hentak": start_hentak(); break;
                case "keluar baris": StartCoroutine(keluarBaris()); break;
                case "tsukalanka": tsukalanka(); break;
                default: break;
            }
    }

    public List<string> availableCommands()
    {
        List<string> commands = new List<string>();
        switch (state)
        {
            case 0:
                if (allSedia()) {
                    commands.Add("ke kenan pu seng");
                    commands.Add("ke kiri pu seng");
                    commands.Add("ke belakang pu seng");
                    if (direction == 1 || direction == 3)
                    {
                        commands.Add("cepat jalan");
                        commands.Add("hentak kaki cepat hentak");
                    }
                    else
                    {
                        
                        
                        if (getManCount() >= 6)
                        {
                            commands.Add("ke kenan lu rus");
                            commands.Add("dalam buka barisan");
                            commands.Add("dalam tutup barisan");
                            commands.Add("dari kenan bilang");
                        }
                        commands.Add("senang diri");
                        commands.Add("keluar baris");

                    }
                    commands.Add("tahat sedia");
                    commands.Add("hormat");
                }
                else
                {
                    commands.Add("sedia");
                }
                
                
                break;
            case 1:
                break;
            case 2:
                if (moveState == 0)
                {
                    commands.Add("tsukalanka");
                }
                    commands.Add("berhenti");
                if (moveState == 0) {
                    //commands.Add("tsukalanka");
                    commands.Add("hentak kaki cepat hentak");
                }
                else if (moveState == 1) { commands.Add("maju"); }
                
                break;
            case 3:
                commands.Add("pandang ke hadapan pandang");
                break;
            default:
                break;
        }
        
        return commands;
    }


    // Start is called before the first frame update
    void Start()
    {
        setManCount(1);
        march_song = march_tempo;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
