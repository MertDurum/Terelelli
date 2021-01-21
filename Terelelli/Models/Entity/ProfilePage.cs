using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Terelelli.Models.Entity
{
    public class ProfilePage
    {
        public Models.Entity.Projects projects { get; set; }
        TaskBoardEntities7 db = new TaskBoardEntities7();

        public List<Projects> UserProjects(string _userId)
        {
            /*var ProjectUserRelationship = db.ProjectUsers.Where(x => x.UserId == _userId).Select(x => new { x.ProjectId, x.UserId }).ToList();
            List<Projects> projects = db.Projects.Where(x => x.ProjectId == ProjectUserRelationship.ProjectId).Select
            */
            string query = String.Format("select P.ProjectId, P.ProjectAuthorId, P.ProjectStartDate, P.ProjectName, P.ProjectDifficulty from Projects P LEFT JOIN ProjectUsers PU ON P.ProjectId = PU.ProjectId WHERE PU.UserId = {0}", _userId);
            var projects = db.Projects.SqlQuery(query).ToList<Projects>();
            return projects;
        }
    }
}