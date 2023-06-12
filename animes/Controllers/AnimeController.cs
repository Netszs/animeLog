using animes.Data;
using animes.Models;
using JikanDotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace animes.Controllers
{
    public class AnimeController : Controller
    {
        private readonly ApplicationDbContext _context;

        IJikan jikan = new Jikan();
        public AnimeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Search(string searchQuery)
        {
            var buscaAnime = await jikan.SearchAnimeAsync(searchQuery);

            if(!searchQuery.IsNullOrEmpty())
            {
                var searchLog = new AnimeSearchLog
                {
                    NomeAnime = searchQuery,
                    DataHora = DateTime.Now
                };

                _context.animeSearchLogs.Add(searchLog);
                _context.SaveChanges();
            }            

            return View("Search", buscaAnime.Data);
        }

        public IActionResult GenerateLogsFile()
        {
            var logs = _context.animeSearchLogs.ToList();
            var fileContent = GenerateLogsFileContent(logs);
            var fileName = "anime_logs.txt";

            return File(Encoding.UTF8.GetBytes(fileContent), "text/pain", fileName);
        }

        private string GenerateLogsFileContent(List<AnimeSearchLog> logs)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Logs de Animes Pesquisados");
            sb.AppendLine("======================================");

            foreach (var log in logs)
            {
                sb.AppendLine($"Nome do Anime: {log.NomeAnime}");
                sb.AppendLine($"Data e Hora da consulta: {log.DataHora}");
                sb.AppendLine("--------------------------------------");
            }

            return sb.ToString();
        }

    }
}
