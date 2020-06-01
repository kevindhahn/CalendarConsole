using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Calendar;


namespace CalendarConsole
{
    class Program
    {
        const int padding = 2;

        static void Main(string[] args)
        {
            //enum CalendarType = {OneMonth, ThreeMonths, FullYear };

            DateTime currentDate = DateTime.Now;

            int calendarType = 0;
            int dayWidth = 2;
            DayOfWeek start = DayOfWeek.Sunday;

            int year = 0;
            int month = 0;
            int day = 0;

            // Parse command line args
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg[0] == '-')
                {
                    // Process output options
                    for (int j = 1; j < arg.Length; j++)
                    {
                        if ((arg[j] == '1') && (calendarType != 0))
                        {
                            calendarType = 0;
                        }
                        else if ((arg[j] == '3') && (calendarType < 1))
                        {
                            calendarType = 1;
                        }
                        else if ((arg[j] == 'y') && (calendarType < 2))
                        {
                            calendarType = 2;
                        }

                        if (arg[j] == 'i')
                        {
                            dayWidth = 3;
                        }
                        
                        if ((arg[j] == 'm') && (start != DayOfWeek.Monday))
                        {
                            start = DayOfWeek.Monday;
                        }
                        else if ((arg[j] == 's') && (start != DayOfWeek.Sunday))
                        {
                            start = DayOfWeek.Sunday;
                        }
                    }
                }
                else
                {
                    // Process other parameters to form date
                    int value;
                    bool result;
                    if (year == 0)
                    {
                        result = Int32.TryParse(arg, out value);
                        if ((result == true) && (value > 1500) && (value <= 2500))
                        {
                            year = value;
                        }
                    }
                    else if (month == 0)
                    {
                        result = Int32.TryParse(arg, out value);
                        if ((result == true) && (value > 0) && (value <= 12))
                        {
                            month = value;
                        }
                    }
                    else if (day == 0)
                    {
                        result = Int32.TryParse(arg, out value);
                        if (result == true)
                        {
                            if ((value > 0) && (value <= DateTime.DaysInMonth(year, month)))
                            {
                                day = value;
                            }
                        }
                    }
                }
            }

            // Create command line date
            if (year > 0)
            {
                if (month > 0)
                {
                    if (day > 0)
                    {
                        currentDate = new DateTime(year, month, day);                        
                    }
                    else
                    {
                        currentDate = new DateTime(year, month, 1);
                    }
                }
                else
                {
                    currentDate = new DateTime(year, 1, 1);
                }
            }

            MyMonth currMonth = new MyMonth(currentDate);
            if ((year > 0) && (day == 0))
            {
                currMonth.HighlightDay = false;
            }

            switch (calendarType){
                case 0:
                    PrintMonth(currMonth, start, dayWidth);
                    break;

                case 1:
                    PrintThreeMonths(currMonth, start, dayWidth, true);
                    break;

                default:
                    PrintWholeYear(currMonth, start, dayWidth);
                    break;
            }

            Console.ReadLine();
        }

        private static void PrintPadding()
        {
            for(int i = 0; i < padding; i++)
            {
                Console.Write(" ");
            }
        }

        private static string BuildHeader(string header, int width)
        {
            string result = header;
            int buffer = (width - header.Length) / 2;

            for (int i = 0; i < buffer; i++)
            {
                result = " " + result;
            }

            for (int i = result.Length; i < width; i++)
            {
                result = result + " ";
            }

            return result;
        }

        private static void PrintDaysOfWeek(DayOfWeek start, int dayWidth)
        {
            for (int i = 0; i < 7; i++)
            {
                string day = start.ToString().Substring(0, dayWidth);
                Console.Write(day + " ");

                if (start == DayOfWeek.Saturday)
                {
                    start = DayOfWeek.Sunday;
                }
                else
                {
                    start++;
                }
            }
        }

        private static void PrintMonth(MyMonth currMonth, DayOfWeek start, int dayWidth)
        {
            string header = currMonth.Date.ToString("MMMM yyyy");
            int width = (dayWidth + 1) * 7;
            Console.WriteLine(BuildHeader(header, width));

            PrintDaysOfWeek(start, dayWidth);
            Console.WriteLine();

            DateTime end = new DateTime(currMonth.Date.Year, currMonth.Date.Month + 1, 1);
            while (currMonth.PrintDate < end)
            {
                currMonth.PrintNextWeek(start, dayWidth);
                Console.WriteLine();
            }
        }

        private static void PrintThreeMonths(MyMonth curr, DayOfWeek start, int dayWidth, bool printYear)
        {
            MyMonth prev = new MyMonth(curr.Date.AddMonths(-1));
            prev.HighlightDay = false;
            MyMonth next = new MyMonth(curr.Date.AddMonths(1));
            next.HighlightDay = false;

            int width = (dayWidth + 1) * 7;
            string header = String.Empty;

            string format = "MMMM";
            if (printYear == true)
            {
                format = format + " yyyy";
            }

            header = prev.Date.ToString(format);
            Console.Write(BuildHeader(header, width));
            PrintPadding();

            header = curr.Date.ToString(format);
            Console.Write(BuildHeader(header, width));
            PrintPadding();

            header = next.Date.ToString(format);
            Console.Write(BuildHeader(header, width));
            PrintPadding();

            Console.WriteLine();

            PrintDaysOfWeek(start, dayWidth);
            PrintPadding();
            PrintDaysOfWeek(start, dayWidth);
            PrintPadding();
            PrintDaysOfWeek(start, dayWidth);
            Console.WriteLine();

            DateTime prevEnd = new DateTime(curr.Date.Year, curr.Date.Month, 1);
            DateTime currEnd = new DateTime(next.Date.Year, next.Date.Month, 1);
            DateTime nextEnd = new DateTime(next.Date.Year, next.Date.Month + 1, 1);

            while ((curr.PrintDate < currEnd) || (prev.PrintDate < prevEnd) || (next.PrintDate < nextEnd))
            {
                prev.PrintNextWeek(start, dayWidth);
                PrintPadding();
                curr.PrintNextWeek(start, dayWidth);
                PrintPadding();
                next.PrintNextWeek(start, dayWidth);
                PrintPadding();

                Console.WriteLine();
            }
        }

        private static void PrintWholeYear(MyMonth curr, DayOfWeek start, int dayWidth)
        {
            string header = curr.Date.Year.ToString();
            Console.Write(BuildHeader(header, ((dayWidth+1)*21)+ 6));
            Console.WriteLine();

            for (int i = 0; i < 4; i++)
            {
                MyMonth first = new MyMonth(new DateTime(curr.Date.Year, (i * 3) + 1, 1));
                if (first.Date.Month != curr.Date.Month)
                {
                    first.HighlightDay = false;
                }
                MyMonth middle = new MyMonth(new DateTime(curr.Date.Year, (i * 3) + 2, 1));
                if (middle.Date.Month != curr.Date.Month)
                {
                    middle.HighlightDay = false;
                }
                MyMonth last = new MyMonth(new DateTime(curr.Date.Year, (i * 3) + 3, 1));
                if (last.Date.Month != curr.Date.Month)
                {
                    last.HighlightDay = false;
                }

                int width = (dayWidth + 1) * 7;

                header = first.Date.ToString("MMMM");
                Console.Write(BuildHeader(header, width));
                PrintPadding();

                header = middle.Date.ToString("MMMM");
                Console.Write(BuildHeader(header, width));
                PrintPadding();

                header = last.Date.ToString("MMMM");
                Console.Write(BuildHeader(header, width));
                PrintPadding();
                Console.WriteLine();

                DateTime firstEnd = first.Date.AddMonths(1);
                DateTime middleEnd = middle.Date.AddMonths(1);
                DateTime lastEnd = last.Date.AddMonths(1);

                while ((first.PrintDate < firstEnd) || (middle.PrintDate < middleEnd) || (last.PrintDate < lastEnd))
                {
                    first.PrintNextWeek(start, dayWidth);
                    PrintPadding();
                    middle.PrintNextWeek(start, dayWidth);
                    PrintPadding();
                    last.PrintNextWeek(start, dayWidth);
                    PrintPadding();

                    Console.WriteLine();
                }
            }
        }
    }
}
