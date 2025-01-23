using Microsoft.AspNetCore.Mvc;
using SuperLigMatchSimulator.Classes;

namespace SuperLigMatchSimulator.ViewComponents
{
    public class _WeekMatchSelectorPartial:ViewComponent
    {
        public IViewComponentResult Invoke(WeekMatch match)
        {

            return View(match);
        }
    }
}
