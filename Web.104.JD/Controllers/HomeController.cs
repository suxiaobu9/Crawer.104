using JD._104.Model.Models.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;
using Web._104.JD.Models;

namespace Web._104.JD.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Crawer104Context db;

    public HomeController(ILogger<HomeController> logger, Crawer104Context db)
    {
        _logger = logger;
        this.db = db;

    }

    public async Task<IActionResult> Index(bool? onlyRemote)
    {
        var companies = await db.Companies
            .AsNoTracking()
            .Include(x => x.JobDescriptions.Where(x=>x.IsDeleted ==null || !x.IsDeleted.Value))
            .Where(x => x.Ignore == null || !x.Ignore.Value)
            .Where(x => x.JobDescriptions.Any(y => y.IsDeleted == null || !y.IsDeleted.Value))
            .OrderByDescending(x => x.JobDescriptions.Any(y => y.IsDeleted != null && !y.IsDeleted.Value && y.RemoteWork != null && y.RemoteWork.Value && y.HaveRead != null && !y.HaveRead.Value))
            .ThenBy(x => x.Id)
            .ToArrayAsync();

        return View(companies);
    }

    public async Task<IActionResult> JD(long compId)
    {
        var company = await db.Companies
            .AsNoTracking()
            .Include(x => x.JobDescriptions.Where(y => y.IsDeleted == null || !y.IsDeleted.Value).OrderBy(y => y.HaveRead == null || !y.HaveRead.Value).ThenByDescending(y => y.RemoteWork != null && y.RemoteWork.Value))
            .FirstOrDefaultAsync(x => x.Id == compId);

        return View(company);
    }

    public async Task<IActionResult> ReadJD(long compId, long jdId)
    {
        var jd = await db.JobDescriptions.FirstOrDefaultAsync(x => x.Id == jdId);

        if (jd == null)
            return RedirectToAction("JD", new { compId });

        jd.HaveRead = true;

        await db.SaveChangesAsync();

        return RedirectToAction("JD", new { compId });

    }

    public async Task<IActionResult> Ignore(long compId)
    {
        var company = await db.Companies.FirstOrDefaultAsync(x => x.Id == compId);

        if (company == null)
            return RedirectToAction("Index");

        company.Ignore = true;

        await db.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> ReadAll(long compId)
    {
        var jobs = await db.JobDescriptions.Where(x => x.CompanyId == compId).ToArrayAsync();

        jobs = jobs.Select(x => { x.HaveRead = true; return x; }).ToArray();

        await db.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> SetPro(long compId, string prop)
    {
        var comp = await db.Companies.FirstOrDefaultAsync(x => x.Id == compId);

        if (comp == null)
            return RedirectToAction("Index");

        comp.Property = prop;

        await db.SaveChangesAsync();

        return RedirectToAction("JD", new { compId });

    }
}