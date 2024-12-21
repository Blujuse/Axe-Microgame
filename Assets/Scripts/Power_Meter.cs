using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Power_Meter : MonoBehaviour
{
    [Header("Movement")]
    public bool upOrDown = true; //whether we are moving up or down
    public bool stopped = false;    //whether we have stopped or not
    public float upSpeed;    //our upwards movement speed
    public float downSpeed;    //our downwards movement speed
   
    [Header("Bar Height")]
    public float maxHeight;    //the max height at which we will change direction
    public float minHeight;    //the min height at which we will change direction
    
    [Header("Max & Min Win")]
    public float num2minWinHeight;    
    public float num2maxWinHeight;

    [Header("Max & Min Losses")]
    public float MiddleThrowMax;
    public float MiddleThrowLow;
    public float LeastThrowMax;
    public float LeastThrowLow;
    public float num1minLossHeight;
    public float num1maxLossHeight;
    public float num3minLossHeight;
    public float num3maxLossHeight;

    [Header("Animations")]
    public Animator HandAni;
    public Animator AxeAni;
    public float AnimationTimerWin;
    public float AnimationTimerLoss;

    [Header("Timer")]
    public Timer time;
    public float StopPlayerThrow;

    void Update()
    {
        if (time.countdownTime < StopPlayerThrow)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {    //if the mouse is clicked
                HandAni.SetBool("ButtonPressed", true);
                stopped = true;    //then stop
                Stopped();
            }
            if (!stopped)
            { //if we haven't stopped
                MoveUpDown(); //move line
            }
        }
    
        if (AnimationTimerWin == 0)
        {
            SceneManager.LoadScene("Win");
        }
        else if (AnimationTimerLoss == 0)
        {
            SceneManager.LoadScene("Lose");
        }

       
    }


    void MoveUpDown()
    {
        if (upOrDown)
        {    //if we are moving up
            transform.Translate(Vector3.up * upSpeed * Time.deltaTime);    //move up
            if (transform.position.y > maxHeight)
            {    //if we are at the max height
                upOrDown = false;    //switch to moving down
            }
        }
        else
        {    //if we are moving down
            transform.Translate(Vector3.down * downSpeed * Time.deltaTime);    //move down
            if (transform.position.y < minHeight)
            {    //if we are at the min height
                upOrDown = true; //switch to moving up
            }
        }
    }


    void Stopped()    //when we have stopped
    {
        if (transform.position.y > num2minWinHeight && transform.position.y < num2maxWinHeight)
        {
            Debug.Log("YOU WIN!!!");    //if line is between win min and max height we win or lose
            AxeAni.SetBool("Winning_Throw", true);
            StartCoroutine(CountdownWin());
        }
        else if (transform.position.y > MiddleThrowLow && transform.position.y < MiddleThrowMax)
        {
            Debug.Log("YOU LOSE!!!, Mid");
            AxeAni.SetBool("Medium_Throw", true);
            StartCoroutine(CountdownLoss());
        }
        else if (transform.position.y > LeastThrowLow && transform.position.y < LeastThrowMax)
        {
            Debug.Log("YOU LOSE!!!, Least");
            AxeAni.SetBool("Least_Throw", true);
            StartCoroutine(CountdownLoss());
        }
        else if (transform.position.y > num1minLossHeight && transform.position.y < num1maxLossHeight)
        {
            Debug.Log("YOU Lose, Too High");    //if line is between win min and max height we win or lose
            AxeAni.SetBool("Max_Throw", true);
            StartCoroutine(CountdownLoss());
        }
        else if (transform.position.y > num3minLossHeight && transform.position.y < num3maxLossHeight)
        {
            Debug.Log("YOU Lose, Slightly Too Low");    //if line is between win min and max height we win or lose
            AxeAni.SetBool("Minimum_Red_Throw", true);
            StartCoroutine(CountdownLoss());
        }
        else
        {
            Debug.Log("YOU LOSE");
            HandAni.SetBool("ButtonPressed", false);
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

    IEnumerator CountdownLoss()
    {
        while (AnimationTimerLoss > 0)
        {
            yield return new WaitForSeconds(1f);

            AnimationTimerLoss--;
        }

    }
}

