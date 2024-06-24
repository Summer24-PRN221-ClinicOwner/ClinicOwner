using System.Data;
using System.Text.RegularExpressions;

namespace BusinessObjects
{
    public class Slot
    {
        public int Key { get; set; }
        public string DisplayTime { get; set; } = "";
        public bool IsAvailable { get; set; } = true;
    }

    public static class SlotDefiner
    {
        public static readonly string startSlot_1Display = "7h00";
        public static readonly string startSlot_2Display = "8h00";
        public static readonly string startSlot_3Display = "9h00";
        public static readonly string startSlot_4Display = "10h00";
        public static readonly string startSlot_5Display = "11h00";
        public static readonly string endSlot_5Display = "12h00";
        public static readonly string startSlot_6Display = "13h00";
        public static readonly string startSlot_7Display = "14h00";
        public static readonly string startSlot_8Display = "15h00";
        public static readonly string startSlot_9Display = "16h00";
        public static readonly string startSlot_10Display = "17h00";
        public static readonly string endSlot_10Display = "18h00";

        public static readonly Slot slot_1 = new() { Key = 1, DisplayTime = "Slot 1 : 7h00 - 8h00" };
        public static readonly Slot slot_2 = new() { Key = 2, DisplayTime = "Slot 2 : 8h00 - 9h00" };
        public static readonly Slot slot_3 = new() { Key = 3, DisplayTime = "Slot 3 : 9h00 - 10h00" };
        public static readonly Slot slot_4 = new() { Key = 4, DisplayTime = "Slot 4 : 10h00 - 11h00" };
        public static readonly Slot slot_5 = new() { Key = 5, DisplayTime = "Slot 5 : 11h00 - 12h00" };
        //Break
        public static readonly Slot slot_6 = new() { Key = 6, DisplayTime = "Slot 6 : 13h00 - 14h00" };
        public static readonly Slot slot_7 = new() { Key = 7, DisplayTime = "Slot 7 : 14h00 - 15h00" };
        public static readonly Slot slot_8 = new() { Key = 8, DisplayTime = "Slot 8 : 15h00 - 16h00" };
        public static readonly Slot slot_9 = new() { Key = 9, DisplayTime = "Slot 9 : 16h00 - 17h00" };
        public static readonly Slot slot_10 = new() { Key = 10, DisplayTime = "Slot 10 : 17h00 - 18h00" };

        public static readonly List<string> sequenceDisplayTime = [startSlot_1Display, startSlot_2Display, startSlot_3Display, startSlot_4Display, startSlot_5Display, endSlot_5Display, startSlot_6Display, startSlot_7Display, startSlot_8Display, startSlot_9Display, startSlot_10Display, endSlot_10Display];
        public static readonly List<Slot> slots = [slot_1, slot_2, slot_3, slot_4, slot_5, slot_6, slot_7, slot_8, slot_9, slot_10];
        public static List<Slot> ConvertFromString(string input)
        {
            var regex = new Regex(@"^[01]{10}$");
            if (input == null || !regex.IsMatch(input)) throw new Exception("Input is not supported");
            var result = new List<Slot>();
            for (int i = 0; i < slots.Count; i++)
            {
                result.Add(NewSlot(i + 1));
                result.ElementAt(i).IsAvailable = input.ElementAt(i) != '0';
            }
            return result.OrderBy(item => item.Key).ToList();
        }
        public static string ConvertToString(List<Slot> input)
        {
            if (input == null || input.Count != 10) throw new Exception("Invalid list, must have 10 unique slot");
            string result = "";
            input = input.OrderBy(x => x.Key).ToList();
            var data = new Dictionary<int, Slot>();
            try
            {
                foreach (Slot slot in input)
                {
                    data.Add(slot.Key, slot);
                }
                foreach (var item in data)
                {
                    result += item.Value.IsAvailable == true ? "1" : "0";
                }
            }
            catch (Exception e)
            {
                throw new Exception("Invalid List, Key duplication!");
            }
            return result;
        }

        public static Slot NewSlot(int key) => new() { Key = key, DisplayTime = slots.ElementAt(key - 1).DisplayTime, IsAvailable = true };
        public static int CheckSlotRequired(string slotString, int slotRequired)
        {

            List<Slot> listSlot = ConvertFromString(slotString);
            int checkedSlot = 0;
            foreach (Slot slot in listSlot)
            {
                if (slot.IsAvailable && (slotRequired == 1 || (slotRequired != 1 && slot.Key != 6)))
                {
                    checkedSlot++;
                    if (checkedSlot == slotRequired)
                    {
                        return slot.Key + 1 - slotRequired;
                    }
                }
                else
                {
                    if (slot.IsAvailable && slot.Key == 6) checkedSlot = 1;
                    else checkedSlot = 0;
                }
            }
            return -1;
        }
        public static int CheckSlotRequired(List<Slot> listSlot, int slotRequired)
        {
            int checkedSlot = 0;
            foreach (Slot slot in listSlot)
            {
                if (slot.IsAvailable && slot.Key != 6)
                {
                    checkedSlot++;
                    if (checkedSlot == slotRequired)
                    {
                        return slot.Key + 1 - slotRequired;
                    }
                }
                else
                {
                    checkedSlot = 0;
                }
            }
            return -1;
        }
        public static List<Slot> DurationDiplayTimeOnSlot(List<Slot> listSlot, int requiredSlot)
        {

            foreach (Slot slot in listSlot)
            {
                if (slot.Key + requiredSlot >= sequenceDisplayTime.Count()) return listSlot;
                if (slot.Key < 6)
                    slot.DisplayTime = sequenceDisplayTime.ElementAt(slot.Key - 1) + " - " + sequenceDisplayTime.ElementAt(slot.Key + requiredSlot - 1);
                else slot.DisplayTime = sequenceDisplayTime.ElementAt(slot.Key) + " - " + sequenceDisplayTime.ElementAt(slot.Key + requiredSlot);
            }
            return listSlot;
        }
    }
}
