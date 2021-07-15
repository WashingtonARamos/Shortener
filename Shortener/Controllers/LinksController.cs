using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Controllers
{
    [Route("/")]
    public class LinksController : ControllerBase
    {
        private readonly ILinkRepository _linksRepository;

        public LinksController(ILinkRepository linksRepository) => _linksRepository = linksRepository;

        [HttpGet("{link}")]
        public async Task<IActionResult> Get(string link)
        {
            Link newLink = await _linksRepository.Get(link);
            if (newLink != null)
                return Redirect(newLink.OriginalUrl);
            return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Link l)
        {
            if (string.IsNullOrEmpty(l.OriginalUrl))
                return BadRequest(new { Message = "Você deve informar um link a ser encurtado" });
            if (!Uri.TryCreate(l.OriginalUrl, UriKind.Absolute, out _))
                return BadRequest(new { Message = "O link informado é inválido" });

            l.Id = RandomString(5);
            await _linksRepository.Add(l);
            return Ok(l);
        }

        #region Overkill random string generator
        private static string RandomString(int length)
        {
            const string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            var res = new StringBuilder();
            using (RNGCryptoServiceProvider rnd = new())
            {
                while (length-- > 0)
                {
                    res.Append(valid[GetInt(rnd, valid.Length)]);
                }
            }
            return res.ToString();
        }

        private static int GetInt(RNGCryptoServiceProvider random, int max)
        {
            byte[] bytes = new byte[4];
            int value;
            do
            {
                random.GetBytes(bytes);
                value = BitConverter.ToInt32(bytes, 0) & int.MaxValue;
            } while (value >= max * (int.MaxValue / max));
            return value % max;
        }
        #endregion

    }
}
