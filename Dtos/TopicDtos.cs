using Clarity_Crate.Models;
using System.ComponentModel.DataAnnotations;

public class TopicCreateDto
{

    [Required]
    public string? Name { get; set; }

    [Required]
    public IEnumerable<Subject> SelectedTopics { get; set; } = new List<Subject>();

}


public class TopicUpdateDto
{

    [Required]
    public string? Name { get; set; }

    [Required]
    public IEnumerable<Subject> SelectedTopics { get; set; } = new List<Subject>();

}