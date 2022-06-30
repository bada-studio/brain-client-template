using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleIndexProvider {
    private int pivot = 0;
    private List<int> indexList;

    public RuleIndexProvider(List<int> indexList) {
        this.indexList = indexList;
        ResetIndex();
    }

    public int GetIndex() {
        if (indexList[pivot] < 0)
            ResetIndex();
        var result = indexList[pivot];
        pivot++;
        return result;
    }

    public void ResetIndex() {
        if (indexList.Contains(-1))
            indexList.Remove(-1);
        indexList.Shuffle();
        indexList.Add(-1);
        pivot = 0;
    }
}