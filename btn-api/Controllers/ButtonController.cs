using System;
using System.Security.Claims;
using System.Threading.Tasks;
using btn_api.Helpers;
using btn_api._Services.Interface;
using btn_api.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using btn_api.Models;

namespace btn_api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ButtonController : ControllerBase
    {
        private readonly IButtonService _btnService;
        public ButtonController(IButtonService btnService)
        {
            _btnService = btnService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lines = await _btnService.GetAllAsync();
            return Ok(lines);
        }
        [HttpGet("{workerID}")]
        public async Task<IActionResult> GetAllButtonByWorkerID(int workerID)
        {
            var lines = await _btnService.GetAllButtonByWorkerID(workerID);
            return Ok(lines);
        }
       
        [HttpPost]
        public async Task<IActionResult> Create(ButtonDto create)
        {

            if (_btnService.GetById(create.ID) != null)
                return BadRequest("Button ID already exists!");
            //create.CreatedDate = DateTime.Now;
            if (await _btnService.Add(create))
            {
                return NoContent();
            }

            throw new Exception("Creating the Button failed on save");
        }

        [HttpPut]
        public async Task<IActionResult> Update(ButtonDto update)
        {
            if (await _btnService.Update(update))
                return NoContent();
            return BadRequest($"Updating Button {update.ID} failed on save");
        }
        [HttpGet("{workerID}")]
        public async Task<IActionResult> UnlinkWorkerWithButton(int workerID)
        {
            return Ok(await _btnService.UnlinkWorkerWithButton(workerID));

        }
        [HttpGet("{workerID}")]
        public async Task<IActionResult> CheckExistWorkerLinkButton(int workerID)
        {
            return Ok(await _btnService.CheckExistWorkerLinkButton(workerID));
        }
        [HttpGet("{btn}")]
        public async Task<IActionResult> UnlinkButtonLinkWorker(int btn)
        {
            return Ok(await _btnService.UnlinkButtonLinkWorker(btn));

        }
        [HttpGet("{btn}")]
        public async Task<IActionResult> CheckExistButtonLinkWorker(int btn)
        {
            return Ok(await _btnService.CheckExistButtonLinkWorker(btn));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _btnService.Delete(id))
                return NoContent();
            throw new Exception("Error deleting the Button");
        }
       
    }
}