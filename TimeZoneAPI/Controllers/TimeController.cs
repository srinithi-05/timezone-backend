using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

[ApiController]
[Route("api/time")]
public class TimeController : ControllerBase
{
    [HttpGet("timezones")]
    public IActionResult GetTimezones()
    {
        var timezones = TimeZoneInfo.GetSystemTimeZones()
                                    .Select(tz => tz.Id)
                                    .ToList();
        return Ok(timezones);
    }

    [HttpGet]
    public IActionResult GetTime(string timezone)
    {
        try
        {
            var localTime = DateTime.Now;

            //TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            // Console.WriteLine(tz.DisplayName); // Output: (UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi
            var selectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timezone);

            //returns the converted date and time as a DateTime object.
            var selectedTime = TimeZoneInfo.ConvertTime(localTime, selectedTimeZone);

            //new { SelectedTime = ... } => anonymous object.
            //sending a response object that has one property
            // final o/p to : {  "SelectedTime": "2025-05-05 18:12:00" }
            return Ok(new
            {
                SelectedTime = selectedTime.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }
        catch (TimeZoneNotFoundException)
        {
            return BadRequest("Invalid timezone specified.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}