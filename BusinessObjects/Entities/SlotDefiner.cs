using System.Data;
using System.Text.RegularExpressions;

namespace BusinessObjects.Entities
{
    public class Slot
    {
        public int Key { get; set; }
        public string DisplayTime { get; set; } = "";
        public bool IsAvailable { get; set; } = true;
    }

    public static class SlotDefiner
    {
        public static readonly Slot slot_1 = new() { Key = 1, DisplayTime = "7h00" };
        public static readonly Slot slot_2 = new() { Key = 2, DisplayTime = "8h00" };
        public static readonly Slot slot_3 = new() { Key = 3, DisplayTime = "9h00" };
        public static readonly Slot slot_4 = new() { Key = 4, DisplayTime = "10h00" };
        public static readonly Slot slot_5 = new() { Key = 5, DisplayTime = "11h00" };
        public static readonly Slot slot_6 = new() { Key = 6, DisplayTime = "1h00" };
        public static readonly Slot slot_7 = new() { Key = 7, DisplayTime = "2h00" };
        public static readonly Slot slot_8 = new() { Key = 8, DisplayTime = "3h00" };
        public static readonly Slot slot_9 = new() { Key = 9, DisplayTime = "4h00" };
        public static readonly Slot slot_10 = new() { Key = 10, DisplayTime = "5h00" };
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
