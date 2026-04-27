using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsApp2
{
    internal class GestureHistory
    {
        public int Id { get; set; }
        public string Gesture { get; set; }
        public int Confidence { get; set; }
        public string CreatedAt { get; set; }

        public GestureHistory(int id, string gesture, int confidence, string createdAt)
        {
            Id = id;
            Gesture = gesture;
            Confidence = confidence;
            CreatedAt = createdAt;
        }
    }
}
