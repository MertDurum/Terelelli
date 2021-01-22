using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Terelelli.Models.Entity
{
    public class ProjectPage
    {
        public Panels panels { get; set; }
        public Projects projects { get; set; }
        public Tasks tasks { get; set; }
        // public Users users = new Users();

        TaskBoardEntities7 db = new TaskBoardEntities7();

        public List<Panels> ProjectPanels(string _projectId)
        {
            string query = String.Format("select * from Panels where ProjectId = {0}", _projectId);
            var panels = db.Panels.SqlQuery(query).ToList<Panels>();
            return panels;
        }

        public List<Tasks> PanelTasks(string _panelId)
        {
            string query = String.Format("select * from Tasks where PanelId = {0}", _panelId);
            var tasks = db.Tasks.SqlQuery(query).ToList<Tasks>();
            return tasks;
        }

        public Users UserFromId(string _userId)
        {
            Users u = db.Users.FirstOrDefault(x => x.UserId.ToString() == _userId);
            return u;
        }

        public string TaskDuration(string _taskId)
        {
            Tasks t = db.Tasks.FirstOrDefault(x => x.TaskId.ToString() == _taskId);
            
            if (t.TaskFinishDate != null)
            {
                return db.TaskCompletionTimes.FirstOrDefault(x => x.TaskId.ToString() == _taskId).CompletionTime + " dk";
            }

            return (DateTime.Now - t.TaskStartDate).Value.Minutes + " dk";
        }

        public string EstimatedTaskDuration(string _taskId)
        {
            var tct = db.TaskCompletionTimes.FirstOrDefault(x => x.TaskId.ToString() == _taskId);

            return null;
        }
    }
}