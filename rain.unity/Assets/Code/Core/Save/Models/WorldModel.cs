using System;
using System.Collections.Generic;

namespace Core.Save.Models
{
    [Serializable]
    public class WorldModel
    {
        public string Id;
        public string Name;
        public DateTime CreateTime;
        public DateTime ExitTime;
        public TimeSpan GameTime;
        
        public Dictionary<string, int> SceneResources;
    }
}