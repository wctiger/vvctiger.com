using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace vvctiger.com_Core
{
    public class Repository
    {
        private vvctigerContext context = new vvctigerContext();

        public void CreateWordPair(WordPair pair)
        {
            context.WordPairs.Add(pair);
            context.SaveChanges();
        }

        public void UpdateWordPair(WordPair pair)
        {
            context.SaveChanges();
        }


    }
}
