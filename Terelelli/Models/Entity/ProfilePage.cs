using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Terelelli.Models.Entity
{
    public class ProfilePage
    {
        public Projects projects { get; set; }
        TaskBoardEntities7 db = new TaskBoardEntities7();

        public List<Projects> UserProjects(string _userId)
        {
            string query = String.Format("select P.ProjectId, P.ProjectAuthorId, P.ProjectStartDate, P.ProjectName from Projects P LEFT JOIN ProjectUsers PU ON P.ProjectId = PU.ProjectId WHERE PU.UserId = {0}", _userId);
            var projects = db.Projects.SqlQuery(query).ToList<Projects>();
            return projects;
        }

        public int GetProjectAuthorId(int _projectId)
        {
            int authorId = (int)db.Projects.FirstOrDefault(x => x.ProjectId == _projectId).ProjectAuthorId;
            return authorId;
        }
    }
}