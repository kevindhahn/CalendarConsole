using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar
{
    public class MyMonth
    {
        private DateTime m_date = DateTime.Now;
        private DateTime m_printDate;
        private bool m_highlightDay = true;

        public MyMonth()
        {
            m_printDate = new DateTime(m_date.Year, m_date.Month, 1);
        }

        public MyMonth(DateTime date)
        {
            m_date = date;
            m_printDate = new DateTime(m_date.Year, m_date.Month, 1);
        }

        private static DayOfWeek PreviousDay(DayOfWeek current)
        {
            DayOfWeek result;

            // Get the last day of the week
            if (current == DayOfWeek.Sunday)
            {
                result = DayOfWeek.Saturday;
            }
            else
            {
                result = current - 1;
            }

            return result;
        }

        private static DayOfWeek NextDay(DayOfWeek current)
        {
            DayOfWeek result;

            // Get the last day of the week
            if (current == DayOfWeek.Saturday)
            {
                result = DayOfWeek.Sunday;
            }
            else
            {
                result = current + 1;
            }

            return result;
        }

        private void PrintDay(int dayWidth)
        {
            if ((m_highlightDay == true) && (m_date.Day == m_printDate.Day))
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Gray;
            }

            if (dayWidth == 2)
            {
                Console.Write(m_printDate.ToString("dd"));
            }
            else
            {
                Console.Write("{0,3}", m_printDate.DayOfYear);
            }
            

            if ((m_highlightDay == true) && (m_date.Day == m_printDate.Day))
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
            }

            return;
        }

        public bool HighlightDay
        {
            get { return m_highlightDay; }
            set { m_highlightDay = value; }
        }

        public DateTime Date
        {
            get { return m_date; }
        }

        public DateTime PrintDate
        {
            get { return m_printDate; }
        }

        public void PrintNextWeek(DayOfWeek start, int dayWidth)
        {
            DayOfWeek dow = start;
            DateTime end;
            if (m_date.Month == 12)
            {
                end = new DateTime(m_date.Year + 1, 1, 1);
            }
            else
            {
                end = new DateTime(m_date.Year, m_date.Month + 1, 1);
            }

            // Build First Week
            if ((m_printDate.Day == 1) && (m_printDate.DayOfWeek != start))
            {
                while (dow != m_printDate.DayOfWeek)
                {
                    for (int i = 0; i < dayWidth; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.Write(" ");
                    dow = NextDay(dow);
                }
            }
            else if (m_printDate < end)
            {
                PrintDay(dayWidth);
                Console.Write(" ");
                m_printDate = m_printDate.AddDays(1);
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    for (int k = 0; k < dayWidth; k++)
                    {
                        Console.Write(" ");
                    }
                    Console.Write(" ");
                }
            }

            dow = start;

            
            while (dow != m_printDate.DayOfWeek)
            {
                if (m_printDate < end)
                {
                    PrintDay(dayWidth);
                    Console.Write(" ");
                }
                else
                {
                    for (int i = 0; i < dayWidth; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.Write(" ");
                }
                m_printDate = m_printDate.AddDays(1);
            }

            return;
        }

    }
}
