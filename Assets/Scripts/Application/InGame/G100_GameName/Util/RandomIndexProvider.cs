using System.Collections.Generic;

namespace BCPG9 {
    /*
        Provide Random Index by Collections cell count.
        All index used or forced call, reset index list.
    */
    public class RandomIndexProvider {
        private int pivot = 0;
        private List<int> indexList;

        public RandomIndexProvider(List<int> indexList) {
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

        // Shuffle index and add -1 to list for check list end.
        public void ResetIndex() {
            if (indexList.Contains(-1))
                indexList.Remove(-1);
            indexList.Shuffle();
            indexList.Add(-1);
            pivot = 0;
        }
    }
}