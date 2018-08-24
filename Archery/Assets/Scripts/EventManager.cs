using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class LevelEvents {
    public enum LevelEventType
    {
        ContinueToNextLevel, SpawnTarget, AddScore, GameOver, RetryGame
    }
    public static event Action ContinueToNextLevel;
    public static event Action GameOver;
    public static event Action SpawnTarget;
    public static event Action AddScore;
    public static event Action RetryGame;
    public static void RaiseLevelEvent(LevelEventType levelEventType)
    {
        switch (levelEventType)
        {
            case (LevelEventType.ContinueToNextLevel):
                if(ContinueToNextLevel != null)
                    ContinueToNextLevel();
                break;
            case (LevelEventType.SpawnTarget):
                if (SpawnTarget != null)
                    SpawnTarget();
                break;
            case (LevelEventType.AddScore):
                if (AddScore != null)
                    AddScore();
                break;
            case (LevelEventType.GameOver):
                if(GameOver != null)
                    GameOver();
                break;
            case (LevelEventType.RetryGame):
                if (RetryGame != null)
                    RetryGame();
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