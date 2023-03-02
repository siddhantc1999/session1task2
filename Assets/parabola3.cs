using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parabola3 : MonoBehaviour
{
    public float maintimer;
    public float y;
    public float x;
    public float z;
    public float Vy;
    public float Vx;
    public float Vz;
    public float h;
    public float g = 9.8f;
    public float range;
    [SerializeField] GameObject initalpos;
    [SerializeField] GameObject finalpos;
/*    public List<Vector3> mylist = new List<Vector3>();*/
    [SerializeField] GameObject refgameobject;
    [Tooltip("this is parabola time, no relation with mover or cube movement keep it above 5")]
    public float reftime;
    public List<Vector3> pointlist = new List<Vector3>();
   

    public float balltime = 0;
    public float perpointtime = 0;
    public double segment = 0;
    public Vector3 targetpos;
    [SerializeField] GameObject mover;
    public int k = 0;
    public Vector3 zdistance;
    public GameObject parent;
    public double counttimer = 0;
    public Vector3 moverdistance;

    public float xthreshold;
    public float ythreshold;
    public float zthreshold;

    /// <summary>
    /// below can be deleted to retain the original code
    /// /////////////////////////////////////
    /// </summary>
    public Vector3 refdistance;
    Vector3 previouspoint;
    bool ischanging = true;
    // Start is called before the first frame update
    void Start()
    {


    }
    // Update is called once per frame
    void Update()
    {
        //present cube position
        moverdistance = mover.transform.position;

        //to keep a track of time 
        maintimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            maintimer = 0;
            for (int j=0;j< parent.transform.childCount;j++)
            {
                Destroy(parent.transform.GetChild(j).gameObject);
            }

            pointlist = new List<Vector3>();
            counttimer = 0;
            segment = 0;
            k = 0;
            initalpos.transform.position = mover.transform.position;
           
          
            randomizedirection();
           


        }

        movingball();



    }


    private void movingball()
    {

        ///turn on later
        ///
        

        //counttimer and maintimer are different , countimer counts the time upto perpointtime while maintimer calculates time in real life
        counttimer += Time.deltaTime;
        if (counttimer <= perpointtime)
        {
            //to get the value of segment from(0-1)
            //counttimer is normalized according to perpointtimer
            segment = counttimer / perpointtime;
            
        }
        if (segment <= 1 && k < pointlist.Count)
        {
            //targetpos tells us the next position we have to reach in the list
            targetpos = new Vector3(initalpos.transform.position.x + pointlist[k].x, initalpos.transform.position.y + (pointlist[k].y), pointlist[k].z + initalpos.transform.position.z);



           //for lerp Vector.lerp(initialposition,finalposition,fractionmoved)
           //in previous point we are keeping a track of initial position
            if (k > 0)
            {
                previouspoint = new Vector3(initalpos.transform.position.x + pointlist[k - 1].x, initalpos.transform.position.y + pointlist[k - 1].y, initalpos.transform.position.z + (pointlist[k - 1].z));
            }
            else
            {
               previouspoint = initalpos.transform.position;
            }


          
            refdistance = Vector3.Lerp(previouspoint, targetpos, (float) segment);
            mover.transform.position = refdistance;
          

             xthreshold= Mathf.Abs(mover.transform.position.x - targetpos.x);
             ythreshold = Mathf.Abs(mover.transform.position.y - targetpos.y);
             zthreshold = Mathf.Abs(mover.transform.position.z - targetpos.z);



                if (Mathf.Max(xthreshold, ythreshold, zthreshold) <= 0.1f)
                {

                   
                    k = k + 1;
                    counttimer = 0;
                    segment = 0;

                }
            

        }
    }



    public void randomizedirection()
        {
            
            finalpos.transform.position = new Vector3(Random.Range(-100, 100), Random.Range(0, 100), Random.Range(-100, 100));
            z = mover.transform.position.z;




            zdistance = new Vector3(0f, 0f, finalpos.transform.position.z) - new Vector3(0f, 0f, initalpos.transform.position.z);
            h = finalpos.transform.position.y - initalpos.transform.position.y;
            range = finalpos.transform.position.x - initalpos.transform.position.x;
            Vx = range / reftime;

          

            Vy = (h + (((g / 2) * reftime * reftime))) / reftime;
            Vz = zdistance.z / reftime;
            for (float i = 0; i <= reftime; i = i + 0.1f)
            {


                y = (Vy * i) + (((-9.8f) * (i) * (i)) / 2);
                x = Vx * i;
                z = Vz * i;

                pointlist.Add(new Vector3((float)x, (float) y, (float)z));

            }
            for (int i = 0; i < pointlist.Count; i++)
            {


                GameObject insiantiatedobject = Instantiate(refgameobject, new Vector3(pointlist[i].x + initalpos.transform.position.x, pointlist[i].y + initalpos.transform.position.y, pointlist[i].z + initalpos.transform.position.z), Quaternion.identity);
                insiantiatedobject.transform.parent = parent.transform;
            }
            perpointtime = balltime / pointlist.Count;
        }
    }
