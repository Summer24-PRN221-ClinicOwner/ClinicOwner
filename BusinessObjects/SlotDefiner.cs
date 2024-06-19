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
        public static readonly Slot slot_1 = new() { Key = 1, DisplayTime = "Slot 1 : 7h00 - 8h00" };
        public static readonly Slot slot_2 = new() { Key = 2, DisplayTime = "Slot 2 : 8h00 - 9h00" };
        public static readonly Slot slot_3 = new() { Key = 3, DisplayTime = "Slot 3 : 9h00 - 10h00" };
        public static readonly Slot slot_4 = new() { Key = 4, DisplayTime = "Slot 4 : 10h00 - 11h00" };
        public static readonly Slot slot_5 = new() { Key = 5, DisplayTime = "Slot 5 : 11h00 - 12h00" };
        public static readonly Slot slot_6 = new() { Key = 6, DisplayTime = "Slot 6 : 13h00 - 14h00" };
        public static readonly Slot slot_7 = new() { Key = 7, DisplayTime = "Slot 7 : 14h00 - 15h00" };
        public static readonly Slot slot_8 = new() { Key = 8, DisplayTime = "Slot 8 : 15h00 - 16h00" };
        public static readonly Slot slot_9 = new() { Key = 9, DisplayTime = "Slot 9 : 16h00 - 17h00" };
        public static readonly Slot slot_10 = new() { Key = 10, DisplayTime = "Slot 10 : 17h00 - 18h00" };

        public static readonly List<Slot> slots = [slot_1, slot_2, slot_3, slot_4, slot_5, slot_6, slot_7, slot_8, slot_9, slot_10];
        public static List<Slot> ConvertFromString(string input)
        {
            var regex = new Regex(@"^[01]{10}$");
            if (input == null || !regex.IsMatch(input)) throw new Exception("Input is not supported");
            var result = new List<Slot>();
            for (int i = 0; i < slots.Count; i++)
            {
                result.Add(new() { Key = i + 1, DisplayTime = slots.ElementAt(i).DisplayTime, IsAvailable = input.ElementAt(i) != 0 });
            }
            return result;
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

        public static Slot NewSlot(int key) => new() { Key = key, DisplayTime = slots.FirstOrDefault(item => item.Key == key).DisplayTime, IsAvailable = true };
    }
}
