using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class LevelEvents {
    public enum LevelEventType
    {
        ContinueToNextLevel
    }
    public static event Action ContinueToNextLevel;
    public static void RaiseLevelEvent(LevelEventType levelEventType)
    {
        switch (levelEventType)
        {
            case (LevelEventType.ContinueToNextLevel):
                if(ContinueToNextLevel != null)
                    ContinueToNextLevel();
                break;
        }
        
    }
}
public static class BowEvents
{
    public enum BowEventType
    {
        ShootArrow, LoadArrow, LandArrow
    }
    public static event Action ShootArrow;
    public static event Action LoadArrow;
    public static event Action LandArrow;
    public static void RaiseBowEvent(BowEventType bowEventType)
    {
        switch (bowEventType)
        {
            case (BowEventType.ShootArrow):
                if (ShootArrow != null)
                    ShootArrow();
                break;
            case (BowEventType.LoadArrow):
                if (LoadArrow != null)
                    LoadArrow();
                break;
            case (BowEventType.LandArrow):
                if (LandArrow != null)
                    LandArrow();
                break;
        }

    }
}
public static class GameStateEvents
{
    public enum GameStateEventType
    {
        SwitchedToLoadArrowPhase, SwitchedToShootArrowPhase
    }
    public static event Action SwitchedToLoadArrowPhase;
    public static event Action SwitchedToShootArrowPhase;
    public static void RaiseLevelEvent(GameStateEventType gameStateEventType)
    {
        switch (gameStateEventType)
        {
            case (GameStateEventType.SwitchedToLoadArrowPhase):
                if (SwitchedToLoadArrowPhase != null)
                    SwitchedToLoadArrowPhase();
                break;
            case (GameStateEventType.SwitchedToShootArrowPhase):
                if (SwitchedToShootArrowPhase != null)
                    SwitchedToShootArrowPhase();
                break;
        }

    }
}