﻿using System;
using TowerFall;
using MonoMod.ModInterop;

namespace TFModFortRiseGameModePlaytag
{
  [ModImportName("com.fortrise.TFModFortRiseGameModePlaytag")]
  public static class PlayTagImport
  {
    public static Func<Modes, bool> IsGameModePlayTag;
    public static Func<int, bool> IsPlayTagCountDownOn;
    public static Func<int, bool> IsPlayerPlayTag;
  }
}