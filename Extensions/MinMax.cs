namespace YNL.Editor.Extensions
{
    [System.Serializable]
    public struct MinMax
    {
        public float Min;
        public float Max;
        public MinMaxType Type;

        public MinMax(MinMaxType type)
        {
            Min = 0;
            Max = 1;
            Type = type;
        }
        public MinMax(float min, float max, MinMaxType type = MinMaxType.Field)
        {
            Min = min;
            Max = max;
            Type = type;
        }

        public bool InRange(float number, bool equalLeft = true, bool equalRight = true)
        {
            if (equalLeft && !equalRight) return number >= Min && number < Max;
            if (!equalLeft && equalRight) return number > Min && number <= Max;
            if (equalLeft && equalRight) return number >= Min && number <= Max;
            return number > Min && number < Max;
        }

        public bool InInterval(float number) => InRange(number, true, true);
        public bool InIntervalLeft(float number) => InRange(number, true, false);
        public bool InIntervalRight(float number) => InRange(number, false, true);
        public bool InSegment(float number) => InRange(number, false, false);
    }

    public enum MinMaxType
    {
        Field, Slider
    }
}