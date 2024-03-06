using System;
using System.Text;
using UnityEngine;

namespace Assets.Nodes
{
    [Serializable]
    public class BlackboardEntry
    {
        public string Name = "";
        public string DefaultValue = "";
        public string CurrentValue;
    }

    public class Blackboard : MonoBehaviour
    {
        public BlackboardEntry[] Entries;

        public void Awake()
        {
            if (Entries == null)
                return;
            foreach (BlackboardEntry e in Entries)
            {
                if (string.IsNullOrEmpty(e.CurrentValue))
                {
                    e.CurrentValue = e.DefaultValue;
                }
            }
        }

        public void GenerateBlackboardHeader(StringBuilder output)
        {
            foreach (var entry in Entries)
            {
                output.AppendLine(string.Format("blackboard[\"{0}\"] = {1}", entry.Name, entry.CurrentValue));
            }
            output.AppendLine("");
        }
    }
}
