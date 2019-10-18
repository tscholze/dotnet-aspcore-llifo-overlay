using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Llifo.Overlay.Hubs;
using Llifo.Overlay.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace Llifo.Overlay.Pages
{
    /// <summary>
    /// Page Model of the /Index aka Root file.
    /// 
    /// It provides a list of board items and the
    /// possiblity to broadcast a change in the selection
    /// of an item via SignalR.
    /// </summary>
    public class IndexModel : PageModel
    {
        #region Public properties

        /// <summary>
        /// Underlying list of board items.
        /// </summary>
        public List<BoardItem> BoardItems;

        #endregion

        #region Private members

        /// <summary>
        /// Underlying hosting information context.
        /// </summary>
        private readonly IHostingEnvironment environment;

        /// <summary>
        /// Underlying board hub context.
        /// </summary>
        private readonly IHubContext<BoardHub> hubContext;

        #endregion

        /// <summary>
        /// Initializer.
        /// 
        /// </summary>
        /// <param name="environment">
        /// Underlying hosting environment that is set via dependency injection.
        /// </param>
        /// <param name="hubContext">
        /// Underlying SignalR hub that is set via dependency injection.
        /// </param>
        public IndexModel(IHostingEnvironment environment, IHubContext<BoardHub> hubContext)
        {
            this.environment = environment;
            this.hubContext = hubContext;
        }

        #region Http Event handler

        /// <summary>
        /// Gets called on Http GET requests.
        /// 
        /// Will setup `BoardItems` member.
        /// </summary>
        public void OnGet()
        {
            // 1. Get file names from web root path + "/content" folder.
            //      In most cases this should be "wwwroot/content".
            //
            // 2. Remove the absolute path including the wwwroot path.
            //
            // 3. Map these paths into `BoardItem` object.
            //
            // 3. The page will now use this member to render the list of 
            //      board items.
            BoardItems = Directory
                .GetFiles(Path.Combine(environment.WebRootPath, "content"))
                .Select(path => path.Replace(environment.WebRootPath, string.Empty))
                .Select(path => new BoardItem
                {
                    Name = Path.GetFileName(path),
                    Path = path
                })
                .ToList();
        }

        /// <summary>
        /// Handles Http-Get action for showing a asset on 
        /// on the overlay view.
        /// </summary>
        /// <param name="id">Web path of the selected board item.</param>
        public async Task<IActionResult> OnGetShowAsync(string? id)
        {
            // Ensure a path is given.
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToPage("Index");
            }

            // Post a SignalR message. In this example, the `StreamOverlay.cshtml`
            // will listen to this broadcast via a simple JavaScript event handdler
            // which is located in `/js/streamOverlay.js`.
            await hubContext.Clients.All.SendAsync("BoardItemChanged", id);
            return RedirectToPage("Index");
        }

        #endregion
    }
}
