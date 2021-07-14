using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GS247.Models.TimeTable
{
    public class TimeTable_Combined
    {
        public int Id { get; set; }
        public string WeekDay { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public int IsBreak { get; set; }
    }
    public class TimeTable_Context
    {
        public List<TimeTable_Combined> TimeTable_Combined { get; set; }
        public List<ClassTiming> ClassTimings { get; set; }
        public List<Weekdays> Weekdays { get; set; }
    }
    public class ClassTiming
    {
        public int Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int IsBreak { get; set; }
        public int DefaultClassTiming { get; set; }
    }
    public class Weekdays
    {
        public int Id { get; set; }
        public string WeekdayName { get; set; }
        public int DefaultWeekdays { get; set; }
    }
    public class SearchBatchCourse
    {
        public int BatchId { get; set; }
        public string BatchName { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }
    public class FullTimeTable
    {
        public string AcadamicYear { get; set; }
        public string Course { get; set; }
        public string Batch { get; set; }
        public string Mode { get; set; }
        public string Day { get; set; }
    }
    public class CourseBatchTeacher
    {
        public string Course { get; set; }
        public string Batch { get; set; }
        public string ClassTeacher { get; set; }
    }
    public class FullTimeTable_Context
    {
        public List<ClassTiming> ClassTimings { get; set; }
        public List<Weekdays> Weekdays { get; set; }
        public CourseBatchTeacher CourseBatchTeacher { get;set; }
        public List<TimeTable_Combined> TimeTableCombined { get; set; }
    }
     public class FullTime_Context
    {
        public FullTimeTable FullTimeTable { get; set; }
        public List<FullTimeTable_Context> FullTimeTable_Contexts { get; set; }
        
    }
    public class Teacher_TimeTable
    {
        public string ClassTiming { get; set; }
        public string CourseName { get; set; }
        public string BatchName { get; set; }
        public string Subject { get; set; }
    }
    public class Teacher_Timetables
    {
        public string DayName { get; set;}
        public List<Teacher_TimeTable> Teacher_TimeTables { get; set; }
    }
    public class Teacher_Timetable_Context
    {
        public string TeacherName { get; set; }
        public long TeacherNumber { get; set; }
        public List<Teacher_Timetables> Teacher_Timetables { get; set; }
    }
    public class TimetableWeekdays
    {
        public int Id { get; set; }
        public string Weekday { get; set; }
        public bool IsEnable { get; set; }
        public int BatchId { get; set; }
    }
    public class TimetableClassTiming
    {
        public int Id { get; set; }
        public int BatchId { get; set; }
        public string Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsBreak { get; set; }
    }
}