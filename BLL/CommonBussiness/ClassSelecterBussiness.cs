using BLL.ClassManagement;
using BLL.TeacherManagement;
using BLL.TeachingManagement.BlockTeaching;
using BLL.TeachingManagement.RegularTeaching;
using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.CommonBussiness
{
    public class ClassSelecterBussiness
    {
        public int CurrentClassType { get; private set; }
        public string CurrentClassID { get; private set; }
        public string CurrentTeacherID { get; private set; }

        private RegularClassManagement _regular;
        private BlockClassManagement _block;
        private RegularTraineeBussiness _regularTrainees;
        private BlockTraineeBussiness _blockTrainees;
        private bool _isUsedForPayment;

        public delegate void ClassTypeChanged(int classType);
        public event ClassTypeChanged ClassTypeChangedEvent;
        public delegate void SelectedClassChanged(ClassModel model);
        public event SelectedClassChanged SelectedClassChangedEvent;
        public delegate void ClassCollectionChanged(List<ClassModel> classes);
        public event ClassCollectionChanged ClassCollectionChangedEvent;
        public delegate void TeacherChanged(string teacherName);
        public event TeacherChanged TeacherChangedEvent;
        public delegate void TraineeCollectionChanged(List<TraineeModel> trainees);
        public event TraineeCollectionChanged TraineeCollectionChangedEvent;

        public ClassSelecterBussiness(RegularClassManagement regular, BlockClassManagement block,
            RegularTraineeBussiness regularTrainees, BlockTraineeBussiness blockTrainees, bool isUsedForPayment = false)
        {
            _regular = regular;
            _regularTrainees = regularTrainees;
            _block = block;
            _blockTrainees = blockTrainees;
            CurrentClassType = -1;
            _isUsedForPayment = isUsedForPayment;

            _regularTrainees.LoadTraineesEvent += trainees => TraineeCollectionChangedEvent?.Invoke(trainees);
            _blockTrainees.LoadTraineesEvnet += trainees => TraineeCollectionChangedEvent?.Invoke(trainees);
        }

        public void ChangeClassType(int classType)
        {
            if (classType != CurrentClassType)
            {
                CurrentClassType = classType;
                CurrentClassID = string.Empty;
                switch ((ClassType)CurrentClassType)
                {
                    case ClassType.Regular:
                        ClassCollectionChangedEvent?.Invoke(_regular.RegularClassCollection.ToList<ClassModel>());
                        break;
                    case ClassType.Block:
                        ClassCollectionChangedEvent?.Invoke(_block.BlockClassCollection.ToList<ClassModel>());
                        break;
                }
                ClassTypeChangedEvent?.Invoke(CurrentClassType);
            }
        }

        public void ChangeClass(string className)
        {
            if (!string.IsNullOrEmpty(className))
            {
                //update current class ID
                CurrentClassID = (ClassType)CurrentClassType == ClassType.Regular ?
                    _regular.RegularClassCollection.Where(c => c.ClassName == className).First().ClassID :
                    _block.BlockClassCollection.Where(c => c.ClassName == className).First().ClassID;

                //update default teacher ID
                CurrentTeacherID = (ClassType)CurrentClassType == ClassType.Regular ?
                    _regular.RegularClassCollection.Where(c => c.ClassName == className).First().TeacherID :
                    _block.BlockClassCollection.Where(c => c.ClassName == className).First().TeacherID;

                //update teacher name in UI
                TeacherChangedEvent?.Invoke(TeacherManagementBussiness.Instance.Teachers.Where(t => t.TeacherID == CurrentTeacherID).First().TeacherName);

                //update trainees
                if ((ClassType)CurrentClassType == ClassType.Regular)
                {
                    _regularTrainees.GetTrainees(CurrentClassID);
                }
                else
                {
                    if (_isUsedForPayment)
                        _blockTrainees.GetTraineesNotPay(CurrentClassID);
                    else
                        _blockTrainees.GetTrainees(CurrentClassID);
                }
                SelectedClassChangedEvent?.Invoke((ClassType)CurrentClassType == ClassType.Regular ?
                    (ClassModel)_regular.RegularClassCollection.Where(c => c.ClassName == className).First() :
                    (ClassModel)_block.BlockClassCollection.Where(c => c.ClassName == className).First());
            }
        }

        public void ChangeTeacher(string teacherName)
        {
            if (!string.IsNullOrEmpty(teacherName))
            {
                CurrentTeacherID = TeacherManagementBussiness.Instance.Teachers.Where(t => t.TeacherName == teacherName).First().TeacherID;
            }
        }

        public void ChangeTrainee(string traineeName)
        {

        }
    }
}
