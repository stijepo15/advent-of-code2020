using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent
{
    class Bag
    {

    }
    class Program
    {
        static void Main(string[] args)
        {
            Day7();
        }

        private static void Day7()
        {
            var list = File.ReadAllLines("../../../day6.txt").ToList();
        }

        private static void Day6()
        {
            var list = File.ReadAllLines("../../../day6.txt").ToList();
            var list2 = new List<string>();
            int total = 0;
            foreach (var line in list)
            {
               
                if (line == "")
                {
                    IEnumerable<char> common = list2[0];
                    foreach (var elem in list2)
                    {
                        common = common.Intersect(elem);
                    }
                    total += common.Count();
                    list2 = new List<string>();
                    continue;
                }
                list2.Add(line);
            }
            //var result = list2
            //                .Select(s => s.Replace("\n", ""))
            //                .Select(s => s.Replace("\r", ""))
            //                .Select(s => s.Distinct().Count())
            //                .Sum();
            Console.WriteLine(total);
        }

        private static void Day5()
        {
            List<string> list = File.ReadAllLines("../../../day5.txt").ToList();
            int max = 0;
            List<int> ids = new List<int>();
            foreach(var line in list)
            {
                int row = 0;
                int row_min = 0;
                int row_max = 127;
                int col_min = 0;
                int col_max = 7;
                KeyValuePair<int, int> columnBounds = new KeyValuePair<int, int>(0, 7);
                int column = 0;
                int seatID = 0;
                foreach (char c in line)
                {
                    if (c == 'F')
                        row_max -= (int)Math.Ceiling((decimal)(row_max-row_min) / 2);
                    if (c == 'B')
                        row_min += (int)Math.Ceiling((decimal)(row_max - row_min) / 2);
                    if (row_max == row_min)
                        row = row_min;
                    if (c == 'L')
                        col_max -= (int)Math.Ceiling((decimal)(col_max-col_min) / 2);
                    if (c == 'R')
                        col_min += (int)Math.Ceiling((decimal)(col_max - col_min) / 2);
                    if (col_max == col_min)
                        column = col_min;
                }
                seatID = row * 8 + column;
                ids.Add(seatID);
                if (seatID > max)
                    max = seatID;
                Console.WriteLine(string.Format("row {0}, column {1}, seat ID {2}.", row, column, seatID));
            }
            ids.Sort();
            int missing = 8;
            int i = 0;
            while(i<ids.Count)
            {
                var range = ids.GetRange(i, 10);
                if (ids.GetRange(i, 10).Sum() % 10 != 5)
                {
                    Console.WriteLine("Aha");
                    break;
                }
                else
                {
                    i += 10;
                }
            }
            
            Console.WriteLine(max);
        }

        private static void Day4()
        {
            List<string> list = File.ReadAllLines("../../../day4.txt").ToList();
            List<string> list2 = new List<string>();
            List<string> requiredKeys = new List<string> { "byr", "iyr","eyr","hgt","hcl","ecl","pid" };
            List<string> requiredEyeColors = new List<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
            List<string> optionalKeys = new List<string> { "cid" };
            int validPassports = 0;
            int validDocuments = 0;
            int j = 0;
            list2.Add("");

            foreach(var line in list)
            {
                if(line == "")
                {
                    j++;
                    list2.Add("");
                    continue;
                }
                list2[j] = list2[j] +(list2[j]=="" ? "" : " ") + line;
            }
            foreach(var line in list2)
            {
                Dictionary<string, string> dict = line.Split(" ").ToDictionary(a => a.Split(":")[0], a => a.Split(":")[1]);
                var required = true;
                var optional = true;
                requiredKeys.ForEach(a =>  required = required && dict.ContainsKey(a));
                if (required)
                {
                    if (int.Parse(dict["byr"]) < 1920 || int.Parse(dict["byr"]) > 2002)
                        continue;
                    if (int.Parse(dict["iyr"]) < 2010 || int.Parse(dict["iyr"]) > 2020)
                        continue;
                    if (int.Parse(dict["eyr"]) < 2020 || int.Parse(dict["eyr"]) > 2030)
                        continue;
                    if (dict["hgt"].Substring(dict["hgt"].Length - 2) != "in" && dict["hgt"].Substring(dict["hgt"].Length - 2) != "cm")
                        continue;
                    if (dict["hgt"].Substring(dict["hgt"].Length - 2) == "in")
                    {
                        if (int.Parse(dict["hgt"].Substring(0, dict["hgt"].Length - 2)) < 59 || int.Parse(dict["hgt"].Substring(0, dict["hgt"].Length - 2)) > 76)
                            continue;
                    }
                    if(dict["hgt"].Substring(dict["hgt"].Length - 2) == "cm")
                    {
                        if (int.Parse(dict["hgt"].Substring(0, dict["hgt"].Length - 2)) < 150 || int.Parse(dict["hgt"].Substring(0, dict["hgt"].Length - 2)) > 193)
                            continue;
                    }
                    if (dict["hcl"][0] != '#' || dict["hcl"].Length != 7 || Regex.Match(dict["hcl"], "^#[a-f0-9]{6}$").Success == false)
                    {
                        continue;
                    }
                    if (!requiredEyeColors.Contains(dict["ecl"]))
                        continue;

                    if (dict["pid"].Length != 9 || Regex.Match(dict["pid"], "^[0-9]{9}$").Length != 9)
                    {
                        var a = Regex.Match(dict["pid"], "[0-9]{9}").Length;
                         continue;
                    }


                    validDocuments++;
                    optionalKeys.ForEach(b => optional = optional && dict.ContainsKey(b));
                    if (optional)
                        validPassports++;
                }
            }
            Console.WriteLine("Valid documents are: " + validDocuments);
            Console.WriteLine("Valid passports are: " + validPassports);
        }

        private static void Day3()
        {
            List<string> list = File.ReadAllLines("../../../day3.txt").ToList();


            int result = 1;
            List<KeyValuePair<int, int>> dict = new List<KeyValuePair<int, int>>();
            dict.Add(new KeyValuePair<int, int>(1, 1));
            dict.Add(new KeyValuePair<int, int>(3, 1));
            dict.Add(new KeyValuePair<int, int>(5, 1));
            dict.Add(new KeyValuePair<int, int>(7, 1));
            dict.Add(new KeyValuePair<int, int>(1, 2));
            
            foreach (KeyValuePair<int,int> kvp in dict)
            {
                int trees = 0;
                int squares = 0;
                int i = 0;
                i+=kvp.Value;
                int j = 0;
                while (i < list.Count())
                {

                    j = (j + kvp.Key) % list[i].Length;
                    if (list[i].ElementAt(j) == '#')
                        trees++;
                    
                    i += kvp.Value;
                }
                Console.WriteLine(trees);
                result*=trees;
            }
            Console.WriteLine(result);
        }

        private static void Day1()
        {
            List<int> list = new List<int>();
            list = File.ReadAllLines("../../../day1.txt").Select(int.Parse).ToList();
            for(var i=0;i<list.Count;i++)
            {
                for (var j = i; j < list.Count; j++)
                    for (var k = j; k < list.Count; k++)
                        if (list[i] + list[j] + list[k] == 2020)
                        {
                            Console.WriteLine(list[i] * list[j]* list[k]);
                            return;
                        }
            }
        }
        private static void Day2()
        {
            List<string> list = new List<string>();
            list = File.ReadAllLines("../../../day2.txt").ToList();
            int counter = 0;
            foreach(string row in list)
            {
                var array = row.Split(" ");
                List<int> bounds = array[0].Split("-").Select(int.Parse).ToList();
                char ch = array[1].ToCharArray()[0];
                string source = array[2];
                int count = 0;
                foreach (char c in source)
                    if (c == ch) count++;
                if (count <= bounds[1] && count >= bounds[0])
                    counter++;
            }
            Console.WriteLine(counter);
            Console.WriteLine("Part2");
            counter = 0;
            foreach (string row in list)
            {
                var array = row.Split(" ");
                List<int> bounds = array[0].Split("-").Select(int.Parse).ToList();
                char ch = array[1].ToCharArray()[0];
                string source = array[2];
                int count = 0;
                if(source[bounds[0]-1]==ch ^ source[bounds[1]-1]==ch)
                    counter++;
            }
            Console.WriteLine(counter);

        }
    }
}
