using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class NameCallingModel
    {
        public string NameCallingID { get; set; }
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public int ClassType { get; set; }
        public string TeacherName { get; set; }
        public string TeacherID { get; set; }
        public DateTime ClassDate { get; set; }
        public List<TraineeModel> TraineesWithCallingState { get; set; }

        private string _presence;

        public string Presence
        {
            get { return _presence; }
            set
            {
                _presence = value;
                _presenceTrainees = value == null ? new List<string>() : value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        }

        private List<string> _presenceTrainees;

        public List<string> PresenceTrainees
        {
            get { return _presenceTrainees; }
            set
            {
                _presenceTrainees = value;
                _presence = string.Empty;
                value.ForEach(t => _presence += (t + "|"));
            }
        }

        private string _leave;

        public string Leave
        {
            get { return _leave; }
            set
            {
                _leave = value;
                _leaveTrainees = value == null ? new List<string>() : value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        }

        private List<string> _leaveTrainees;

        public List<string> LeaveTrainees
        {
            get { return _leaveTrainees; }
            set
            {
                _leaveTrainees = value;
                _leave = string.Empty;
                value.ForEach(t => _leave += (t + "|"));
            }
        }

        private string _absence;

        public string Absence
        {
            get { return _absence; }
            set
            {
                _absence = value;
                _absenceTrainees = value == null ? new List<string>() : value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        }

        private List<string> _absenceTrainees;

        public List<string> AbsenceTrainees
        {
            get { return _absenceTrainees; }
            set
            {
                _absenceTrainees = value;
                _absence = string.Empty;
                value.ForEach(t => _absence += (t + "|"));
            }
        }

        private string _giving;

        public string Giving
        {
            get { return _giving; }
            set
            {
                _giving = value;
                _givingTrainees = value == null ? new List<string>() : value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        }

        private List<string> _givingTrainees;

        public List<string> GivingTrainees
        {
            get { return _givingTrainees; }
            set
            {
                _givingTrainees = value;
                _giving = string.Empty;
                value.ForEach(t => _giving += (t + "|"));
            }
        }

    }
}
