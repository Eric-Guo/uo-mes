using System;

namespace UO_Model
{
    /// <summary>
    /// Contain all constant in UO_Model
    /// </summary>
    public static class Constants
    {
        static public DateTime NullDateTime = DateTime.Parse("1981-03-17T00:00:00");  // Different DB need common Null Date
    }

    public enum ActionType { None, Insert, Update, Delete, };
}
