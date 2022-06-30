using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCounter {
    public int comboCount;
    private Timer timer;
    private float comboTime;
    private float lastCheckTime;

    public ComboCounter(float comboTime, Timer timer) {
        this.timer = timer;
        this.comboTime = comboTime;
        comboCount = 0;
        lastCheckTime = timer.time;
    }

    public bool CheckCombo() {
        var isCombo = timer.time - lastCheckTime < comboTime || comboCount <= 0;
        if (isCombo)
            comboCount++;
        else
            comboCount = 0;
        lastCheckTime = timer.time;
        return isCombo;
    }

    public void ResetCounter() {

    }
}