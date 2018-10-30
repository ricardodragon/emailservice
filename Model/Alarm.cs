


namespace emailservice.Model{
    public class Alarm {        
        public int alarmId { get; set; }         
        public int thingId { get; set; }        
        public Thing thing { get; set; }                
        public string alarmDescription { get; set; }                
        public string alarmName { get; set; }                
        public string alarmColor { get; set; }        
        public int priority { get; set; }        
        public long datetime { get; set; }
    }
}