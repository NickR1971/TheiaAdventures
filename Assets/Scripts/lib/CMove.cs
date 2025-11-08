using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/********************************************
 * class CMove
 * алгоритм для розрахунку плавного руху
 * між двома позиціями
 * розрахунок спирається на встановлений час
 * руху від стартової позиції до кінцевої
 ********************************************/

public class CMove
{
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private Vector3 currentPosition;
    private float actionSpeed;
    private CTimer actionTimer;
    private float topJump = -1.0f;
    private float jumpA;
    private float jumpB;

    //---------------------------------------------------------------------------
    // конструктор встановлює час дії на 1 секунду за замовчуванням 
    public CMove()
    {
        actionSpeed = 0;
        actionTimer = new CTimer();
        startPosition = new Vector3(0, 0, 0);
        targetPosition = startPosition;
    }

    //---------------------------------------------------------------------------
    // calcActionTime
    // розрахунок часу дії відповідно до швидкості та відстані між позиціями
    // швидкість нуль або менше ігнорується
    private void CalcActionTime()
    {
        if (actionSpeed > 0)
        {
            actionTimer.SetActionTime(Vector3.Distance(startPosition, targetPosition) / actionSpeed);
        }
    }

    //---------------------------------------------------------------------------
    // SetPositions
    // встановлюємо маршрут руху
    // та перераховуємо час дії якщо встановлена швидкість
    public void SetPositions(Vector3 _start, Vector3 _target, float _topJump = -1.0f)
    {
        startPosition = _start;
        targetPosition = _target;
        topJump = _topJump;
        if (_topJump > 0)
        {
            float hmax, h;
            float b, c;
            float x1, x2;
            hmax = _topJump - _start.y;
            h = _target.y - _start.y;
            b = (4.0f * hmax) - (2.0f * h);
            c = h * h;
            if (CUtil.SolveQuadraticEcuation(1.0f, b, c, out x1, out x2))
            {
                //CUtil.LogConsole("x1=" + x1 + " x2=" + x2);
                if (x2 < x1) jumpA = x2;
                else jumpA = x1;
                jumpB = h - jumpA;
            }
            else
            {
                CUtil.LogConsole("jump failed");
                topJump = -1.0f;
            }
        }
        CalcActionTime();
    }
    //---------------------------------------------------------------------------
    // SetActionTime
    public void SetActionTime(float _actionTime) => actionTimer.SetActionTime(_actionTime);

    //---------------------------------------------------------------------------
    // SetActionSpeed
    // встановлюємо час дії згідно заданої швидкості
    public void SetActionSpeed(float _actionSpeed)
    {
        actionSpeed = _actionSpeed;
        CalcActionTime();
    }

    //---------------------------------------------------------------------------
    // StartAction
    public void StartAction()
    {
        actionTimer.StartAction();
        currentPosition = startPosition;
    }

    //---------------------------------------------------------------------------
    // isActive
    // перевірка чи активна дія на цей час
    public bool IsActive() { return actionTimer.IsActive(); }

    //---------------------------------------------------------------------------
    // GetCurrentPosition
    // отримуємо поточну позицію
    public Vector3 GetCurrentPosition() => currentPosition;

    //---------------------------------------------------------------------------
    // UpdatePosition
    // розраховує поточну позицію та повертає false коли рух завершено
    public bool UpdatePosition()
    {
        if (actionTimer.UpdateState())
        {
            float t = actionTimer.GetState();
            currentPosition = Vector3.Lerp(startPosition, targetPosition, t);
            if (topJump > 0)
            {
                currentPosition.y = (jumpA * t * t) + (jumpB * t) + startPosition.y;
            }
        }
        else
            currentPosition = targetPosition;

     return IsActive();
    }

    //---------------------------------------------------------------------------
    // CorrectTargetPosition
    // Корекція координат призначення використовується якщо ціль рухається
    public void CorrectTargetPosition(Vector3 _target) => targetPosition = _target;

}
