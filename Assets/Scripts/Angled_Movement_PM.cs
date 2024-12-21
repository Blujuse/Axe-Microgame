using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Angled_Movement_PM : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public bool upOrdown = false;
    public bool stopped = false;

    [Header("Y Positions")]
    public float max;
    public float min;
    
    [Header("Angle")]
    public float Angle;

    [Header("Winning Area")]
    public float WinHigh;
    public float WinLow;

    [Header("Losing Areas")]
    public float GreenHigh;
    public float GreenLow;
    public float YellowHigh;
    public float YellowLow;
    public float RedHigh1;
    public float RedLow1;
    public float RedHigh2;
    public float RedLow2;

    [Header("Animations")]
    public Animator HandAni;
    public Animator AxeAni;
    public float AnimationTimerWin;
    public float AnimationTimerLoss;
    public Animator Elevator;
    public Animator Door_L;
    public Animator Door_R;
    public Animator ResizeSprite;

    [Header("Timer")]
    public Timer time;
    public float StopPlayerThrow;

    [Header("Sound")]
    public AudioSource SoundMaker;
    public AudioClip AxeHit;
    public float AxeHitSoundDelay;
    public bool AxeSoundNotPlay = false;
    public AudioClip FlickSound;
    public bool FlickAudio = false;
    public AudioClip Ding;
    public AudioClip Win;
    public AudioClip Lose;
    public bool WinLoseAudio = false;
    public AudioClip AmbienceBGM;
    public bool AmbienceAudio = false;
    public AudioSource AmbienceMaker;
    public AudioClip StoneHit;
    public bool StoneAudio = false;
    public float StoneHitDelayNum;
    public AudioClip ElevatorDoors;
    public float ElevatorDoorDelayNum;
    public bool ElevatorAudio = false;

    [Header("Elevator Squence")]
    public bool SpaceStart = false;
    public GameObject SpaceBar;

    private void Update()
    {
        Move();
       
        Throw();

        AxeSoundDelay();

        ElevatorFunc();

        StoneHitDelay();

        ElevatorSoundDelay();

        Restart();
    }
   
    // Update is called once per frame
    void Move()
    {
        Vector2 VectorFromAngle(float theta)
        {
            return new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
        }

        if (upOrdown)
        {
            transform.Translate(VectorFromAngle(Angle) * speed * Time.deltaTime);

            if (transform.position.x > max)
            {
                upOrdown = false;
            }
        }
        else
        {
            transform.Translate(-VectorFromAngle(Angle) * speed * Time.deltaTime);

            if (transform.position.x < min)
            {
                upOrdown = true;
            }
        }
    }

    void Throw()
    {
        if(SpaceStart == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {   //if the mouse is clicked
                HandAni.SetBool("ButtonPressed", true);
                stopped = true;    //then stop
                Stopped();
                speed = 0;
                if (FlickAudio == false)
                {
                   SoundMaker.PlayOneShot(FlickSound, 0.5f);
                   FlickAudio = true;
                }
            }

            StartCoroutine(ElevatorDoorSound());

            if (AmbienceAudio == false)
            {
                AmbienceMaker.PlayOneShot(AmbienceBGM, 0.2f);
                AmbienceAudio = true;
            }

            if (!stopped)
            {
                 Move();
            }

            Elevator.SetTrigger("Level");
            Door_L.SetTrigger("Door_Move_L");
            Door_R.SetTrigger("Door_Move_R");
            ResizeSprite.SetTrigger("Resize");
        }

        if (AnimationTimerWin == 0)
        {
            AmbienceMaker.Stop();
            ResizeSprite.SetTrigger("Resize_Reverse");
            Door_L.SetTrigger("Door_Move_L_Reverse");
            Door_R.SetTrigger("Door_Move_R_Reverse");
            Elevator.SetTrigger("Win");
            if (WinLoseAudio == false)
            {
                SoundMaker.PlayOneShot(Win, 0.5f);
                WinLoseAudio = true;
            }
        }
        else if (AnimationTimerLoss == 0)
        {
            AmbienceMaker.Stop();
            ResizeSprite.SetTrigger("Resize_Reverse");
            Door_L.SetTrigger("Door_Move_L_Reverse");
            Door_R.SetTrigger("Door_Move_R_Reverse");
            Elevator.SetTrigger("Lose");
            if (WinLoseAudio == false)
            {
                SoundMaker.PlayOneShot(Lose, 0.5f);
                WinLoseAudio = true;
            }
        }
    }

    void AxeSoundDelay()
    {
        if (AxeHitSoundDelay <= 0 && AxeSoundNotPlay == false)
        {
            SoundMaker.PlayOneShot(AxeHit, 0.7f);
            AxeSoundNotPlay = true;
        }
    }
    
    void StoneHitDelay()
    {
        if (StoneHitDelayNum <= 0 && StoneAudio == false)
        {
            SoundMaker.PlayOneShot(StoneHit, 1f);
            StoneAudio = true;
        }
    }

    void ElevatorSoundDelay()
    {
        if (ElevatorDoorDelayNum <= 0 && ElevatorAudio == false)
        {
            SoundMaker.PlayOneShot(ElevatorDoors, 1f);
            ElevatorAudio = true;
        }
    }

    void Stopped()
    {
        if (transform.position.x > WinLow && transform.position.x < WinHigh)
        {
            AxeAni.SetBool("Winning_Throw", true);
            StartCoroutine(CountdownWin());
            StartCoroutine(AxeHitSound());
        }
        else if (transform.position.x > GreenLow && transform.position.x < GreenHigh)
        {
            StoneHitDelayNum = 2.5f;
            AxeAni.SetBool("Least_Throw", true);
            StartCoroutine(CountdownLoss());
            StartCoroutine(StoneHitSound());
        }
        else if (transform.position.x > YellowLow && transform.position.x < YellowHigh)
        {
            StoneHitDelayNum = 3;
            AxeAni.SetBool("Medium_Throw", true);
            StartCoroutine(CountdownLoss());
            StartCoroutine(StoneHitSound());
        }
        else if (transform.position.x > RedLow1 && transform.position.x < RedHigh1)
        {
            StoneHitDelayNum = 3.5f;
            AxeHitSoundDelay = 3f;
            AxeAni.SetBool("Minimum_Red_Throw", true);
            StartCoroutine(CountdownLoss());
            StartCoroutine(StoneHitSound());
            StartCoroutine(AxeHitSound());
        }
        else if (transform.position.x > RedLow2 && transform.position.x < RedHigh2)
        {
            AxeAni.SetBool("Max_Throw", true);
            StartCoroutine(CountdownLoss());
        }
        else
        {
            Debug.Log("YOU LOSE");
            HandAni.SetBool("ButtonPressed", false);
        }
    }

    void ElevatorFunc()
    {
        if (Input.GetKey(KeyCode.Space) && SpaceStart == false)
        {
            SpaceStart = true;
            SpaceBar.SetActive(false);
            SoundMaker.PlayOneShot(Ding, 1f);
        }
    }
   
    IEnumerator CountdownWin()
    {
        while (AnimationTimerWin > 0)
        {
            yield return new WaitForSeconds(1f);

            AnimationTimerWin--;
        }

    }

     public IEnumerator CountdownLoss()
     {
        while (AnimationTimerLoss > 0)
        {
            yield return new WaitForSeconds(1f);

            AnimationTimerLoss--;
        }

     }

    IEnumerator AxeHitSound()
    {
        while (AxeHitSoundDelay > 0)
        {
            yield return new WaitForSeconds(0.2f);

            AxeHitSoundDelay--;
        }
    }

    IEnumerator StoneHitSound()
    {
        while(StoneHitDelayNum > 0)
        {
            yield return new WaitForSeconds(0.1f);

            StoneHitDelayNum--;
        }
    }

    IEnumerator ElevatorDoorSound()
    {
        while (ElevatorDoorDelayNum > 0)
        {
            yield return new WaitForSeconds(0.1f);

            ElevatorDoorDelayNum--; 
        }
    }

    void Restart()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
