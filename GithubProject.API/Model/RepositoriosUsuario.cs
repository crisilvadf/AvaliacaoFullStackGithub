﻿using System.Collections.Generic;

namespace GithubProject.API.Model
{
    public class RepositoriosUsuario
    {
        public int id { get; set; }
        public string name { get; set; }
        public string html_url { get; set; }
        public bool repo_private { get; set; }
    }
}
