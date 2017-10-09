using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//0 unchecked, 1 - open, 2- closed   *******************

public class BlockMethodsNoSuccess : MonoBehaviour
{
    public Block startb;
    public List<Block> endsL = new List<Block>();
    public List<Block> deadEndsL = new List<Block>();
    // might need to set aside 3 manually for easy, medium, hard
    // (or just leave it as is)
    public int size = 12;
    public Block[,] Maze = new Block[13, 13]; //  this would need to be edited
    public Random rnd = new Random();
    public int killswitch;


    // Use this for initialization
    void Start()
    {

        SetUp();
        Vector3 g = new Vector3(0.0f, -20.0f, 0.0f);
        Physics.gravity = g;

        // Debug.Log(Maze[0, 0].status);     
        //Debug.Log(Maze[1, 1].e.row);       
        //Maze[1, 1].s = Maze[1,0];
        Maze[1, 1].prev = Maze[1, 0];
        Maze[1, 1].status = 1;
        
        endsL.Add(Maze[1, 1]); // starts at 1,1 for testing pruposes
        int counter = 0;
        killswitch = 500;
        while (endsL.Count != 0 && killswitch > 0)
        {
            //killswitch--;
            counter++;
            AddNext();
            if(endsL.Count == 0)
            {
                transfer();
            }

        }
        // WHILE 
        Debug.Log("Number of blocks made: " + counter);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                fillIn(Maze[i, j]);
            }
        }
        Block max = deadEndsL[0];
        int count = deadEndsL.Count;
        for (int i = 0; i < count; i++)
        {
            if (deadEndsL[i].sinceStart > max.sinceStart)
            {
                max = deadEndsL[i];
            }

        }
        GameObject flag = GameObject.Find("Flag");
        Vector3 location = new Vector3(max.row * 120, 30, max.clm * 120);
        Quaternion rotation = flag.transform.rotation;
        GameObject.Instantiate(flag, location, rotation);




    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetUp()
    {


        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Maze[i, j] = ScriptableObject.CreateInstance<Block>();
            }
        }

        for (int h = 0; h < size; h++) //  theres a chance h v are reversed, but i can make force it to be like this
        {
            for (int v = 0; v < size; v++)
            {
                Block ptr = Maze[h, v];
                ptr.row = h;
                ptr.clm = v;
                if ((h - 1) > 0)
                {
                    ptr.w = Maze[h - 1, v];
                }
                if ((h + 1) < size)
                {
                    ptr.e = Maze[h + 1, v];
                }
                if ((v - 1) > 0)
                {
                    ptr.s = Maze[h, v - 1];
                }
                if ((v + 1) < size)
                {
                    ptr.n = Maze[h, v + 1];
                }
            }
        }

    }


    //0 unchecked, 1 - open, 2- closed   *******************

    public void AddNext()
    {
     //   Debug.Log("Start AddNext");
        int r = Random.Range(0, endsL.Count);
        //int r = 0; // gonna try always taking first element out of list
        Block fill = endsL[r];
        Debug.Log("Block " + fill.row + " " + fill.clm);
        endsL.Remove(endsL[r]);
        // N, E, S, W
        int[] dir = { 0, 0, 0, 0 };
        
        
        // 0 - unchecked, 1 - open, 2 - closed
        Block[] compass = { fill.n, fill.e, fill.s, fill.w };


        for (int i = 0; i < 4; i++)
        {
            Block ptr = compass[i];

            if (ptr == null)
            {
             //   Debug.Log(i + " found null");
                dir[i] = 2; // fills in invlid directions
            }

            else
            {
                int pts = ptr.status;
                //  Debug.Log("Ptr status of " + i + " is " + pts);
                if (pts != 0) // TESTING*******************
                {
                    dir[i] = 2; // fills in if this square is already open
                                //     Debug.Log("found a filled square (?)");
                }
                else
                {
                    //        Debug.Log("Set to 0");
                    dir[i] = 0;
                }
                if (ptr == fill.prev)
                {
                    Debug.Log(i + " set to Prev");
                    dir[i] = 1; // opens up previous, and overwrites previous command
                }

            }

        } //for loop

        int numz = 0; // number of unfilled squares
        for (int ct = 0; ct < 4; ct++)
        {
            if (dir[ct] == 0)
            {
                numz++;
            }
        }

       // Debug.Log("before fill-in dir " + dir[0] + " " + dir[1] + " " + dir[2] + " " + dir[3]);
        if (numz == 0)
        {
            Debug.Log("This block is closed off");         
            // ELSE, NOTHING, NO MORE PASSAGES TO OPEN OR ANYTHING
            // THIS WILL LIKELY CHANGE
        }

        //MAY NEED ELSE STATEMENT HERE, or just a return above it  - more likely

        // FIgures out whether to closeoff
        bool closeOff = false;
        int sinceEnd = fill.sinceDead;
      //  Debug.Log("sinceDead " + fill.sinceDead);
        if (true) // BYPASS THIS IF - IT WAS IF NOT SUCCESS
        {
            if (sinceEnd > 3)
            {
                int rand1 = Random.Range(1, 10000); //  my hope is that this actually works, we'll see
                int weight1 = ((sinceEnd - 4) / 10) + 1;

                if (rand1 * weight1 > 5000) // weighing adjusted here
                {
                    Debug.Log("CloseOff");
                    closeOff = true;
                    fill.sinceDead = 0;
                }
                else
                {
          //          Debug.Log("Not CloseOff");

                }
            }

        } // triple if, (this is end of the third one)
        if (endsL.Count < 2) // MIGHT MAKE THIS < 2          ****************
        {
            closeOff = false;
        }
        if (numz == 0)
        {
            closeOff = true;
            fill.closedOff = true;
        }
        if (closeOff == true)
        {
            for (int co = 0; co < 4; co++)
            {
                if (dir[co] == 0)
                {
                    dir[co] = 2;
                }

            }
            numz = 0;
        }

        if (numz == 1)
        {
            Debug.Log("1 empty");
            for (int w = 0; w < 4; w++)
            {
                if (dir[w] == 0)
                {


                    fill.next1 = compass[w];
                    dir[w] = 1; // ************************* 
                                // This opens the unfilled

                    // MIGHT NEED TO SET UP END HERE, WE'LL SEE

                }
            }
        }


        bool split = false; // weighted random for if it should split. The further its been, the higher it goes
        int sinceSplit = fill.sinceSplit;
        if (sinceSplit > 3)
        {
            int spl = Random.Range(1, 10000); //  my hope is that this actually works, we'll see
            int weight2 = ((sinceSplit - 4) / 10) + 1;
            if (spl * weight2 > 5000)
            {
                split = true;
           //     Debug.Log("split");
                fill.sinceSplit = 0;
            }

        }
        int selected = Random.Range(0, numz);
     //   Debug.Log("selected " + selected);

        // REMEMBER TO INCREMENT STUFF (since split and all that)

        // FILL IN HERE FOR TWO AND THREE

        if (numz == 2)
        {
            // Debug.Log("2 unfilled");
            if (split == true)
            {
                Debug.Log("2 with split");

                for (int g = 0; g < 4; g++)
                {
                    if (dir[g] == 0)
                    {


                        dir[g] = 1;
                        if (fill.next1 == null)
                        {

                            fill.next1 = compass[g];
                        }
                        else
                        {
                            fill.next2 = compass[g];
                        }
                        // This opens the unfilled
                    }
                }


            }
            // for above, all zeros become open

            else
            { // else for not split
                Debug.Log("2 no split");
                int cnttns = 0;
                for (int twons = 0; twons < 4; twons++)
                {
                    if (dir[twons] == 0)
                    {
                        if (cnttns == selected)
                        {
                            fill.next1 = compass[twons]; // THIS IS FILLED IN NOW, AGAIN, MIGHT NEEED TO END IT HERE, BUT WE'LL SEE
                            dir[twons] = 1;
                            cnttns++;
                        }
                        else
                        {
                            dir[twons] = 2; // make sure other thing is set to closed
                            cnttns++;
                        }
                    }
                }
            } // else for not split
        }

        if (numz == 3)
        {
            Debug.Log("3 unfilled");

            if (split == true)
            {
                Debug.Log("3 split");
                int cntths = 0;
                for (int ths = 0; ths < 4; ths++)
                {
                    if (dir[ths] == 0)
                    {
                        if (cntths == selected)
                        {
                            dir[ths] = 2; // make sure other thing is set to closed
                            cntths++;
                        }
                        else
                        {
                            if (fill.next1 == null)
                            {
                                fill.next1 = compass[ths]; // THIS IS FILLED IN NOW, AGAIN, MIGHT NEEED TO END IT HERE, BUT WE'LL SEE
                                dir[ths] = 1;
                                cntths++;

                            }
                            else
                            {
                                fill.next2 = compass[ths]; // THIS IS FILLED IN NOW, AGAIN, MIGHT NEEED TO END IT HERE, BUT WE'LL SEE
                                dir[ths] = 1;
                                cntths++;
                            }
                        }
                    }
                }
                // MIGHT NEED TO SET UP END HERE, WE'LL SEE
                // OPEN BOTH
                // CLOSE SELECTED, OPEN OTHER TWO
            }

            else
            {
                Debug.Log("3 not split");
                int cntthns = 0;
                for (int thns = 0; thns < 4; thns++)
                {
                    if (dir[thns] == 0)
                    {
                        if (cntthns == selected)
                        {
                            dir[thns] = 1;
                            fill.next1 = compass[thns]; // THIS IS FILLED IN NOW, AGAIN, MIGHT NEEED TO END IT HERE, BUT WE'LL SEE
                            cntthns++;
                        }
                        else
                        {
                            dir[thns] = 2; // make sure other thing is set to closed
                            cntthns++;
                        }
                    }
                }
            }
        } // numz = 3

        /*
        int success = 0;
        if (fill.success == true)
        {
            if (fill.next2 != null)
            {
                success = Random.Range(1, 2);
            }
            else if (fill.next1 != null)
            {
                success = 1;
            }
        }
        */

        if (closeOff == true)
        {
            deadEndsL.Add(fill);
        }

        if (fill.next1 != null) // this is procedurtal stuff to link the linked list style
        {
            fill.next1.prev = fill;
            fill.next1.status = 1;
            fill.next1.sinceDead = fill.sinceDead + 1;
            fill.next1.sinceSplit = fill.sinceSplit + 1;
            fill.next1.sinceStart = fill.sinceStart + 1;
            /*
            if (success == 1)
            {
                fill.next1.success = true;
                fill.next1.sinceDead = 0;
            }
            */
            endsL.Add(fill.next1);

        }
        if (fill.next2 != null)
        {
            fill.next2.prev = fill;
            fill.next2.status = 1;
            fill.next2.sinceDead = fill.sinceDead + 1;
            fill.next2.sinceSplit = fill.sinceSplit + 1;
            fill.next2.sinceStart = fill.sinceStart + 1;
            endsL.Add(fill.next2);
            /*
            if (success == 2)
            {
                fill.next2.success = true;
                fill.next2.sinceDead = 0;
            }
            */
        }

        //0 unchecked, 1 - open, 2- closed   *******************


        fill.status = 2; //  checks box as filled, and not as an end, this is towards the end


        Debug.Log("dir " + dir[0] + " " + dir[1] + " " + dir[2] + " " + dir[3]);
        //  Debug.Log("Reaches End");
        // BUILD VECTOR - IT NEEEDS TO BE HERE, BC THIS IS WHERE DIR IS
        // MAYBE I'LL MOVE IT LATER
        fill.dir = dir;


        /*
        if (fill.success == true)
        {
            Vector3 location = new Vector3(h, 0, v);
            GameObject.Instantiate(sphere, location, rotation90);
        }
        */
    }

    public void fillIn(Block fill)
    {
        int[] dir = fill.dir;
        int offset = 60;
        GameObject wall = GameObject.Find("Wall");
        GameObject sphere = GameObject.Find("Sphere");
        GameObject wall90 = GameObject.Find("Wall90");
        Quaternion rotation = wall.transform.rotation;
        Quaternion rotation90 = wall90.transform.rotation;

        //Debug.Log("h " + fill.row + " " + fill.clm);

        int h = fill.row * 120;
        int v = fill.clm * 120;

        if (dir[0] == 2)
        {
            Vector3 location = new Vector3(h, offset, v + offset);
            GameObject.Instantiate(wall, location, rotation90);

        }
        if (dir[1] == 2)
        {
            Vector3 location = new Vector3(h + offset, offset, v);
            GameObject.Instantiate(wall, location, rotation);

        }
        if (dir[2] == 2)
        {
            Vector3 location = new Vector3(h, offset, v - offset);
            GameObject.Instantiate(wall, location, rotation90);

        }
        if (dir[3] == 2)
        {
            Vector3 location = new Vector3(h - offset, offset, v);
            GameObject.Instantiate(wall, location, rotation);

        }
    }

    public void transfer()
    {
        Block max = deadEndsL[0];
        int count = deadEndsL.Count;
        for (int i = 0; i < count; i ++)
        {
            if(deadEndsL[i].closedOff == false)
            {
                if(deadEndsL[i].sinceStart > max.sinceStart)
                {
                    max = deadEndsL[i];
                }
            }
        }
        if(max.closedOff == false)
        {
            deadEndsL.Remove(max);
            endsL.Add(max);
        }

    }

}

