
# Resume and Website Generator

A program that can generate a resume tuned to a job application using AI, and can output the resume as a PDF or html webpage.

## Overview

### Motivation

Rewriting and reformatting a resume for each job application is tedious, especially when applying for dozens of jobs. So what if there was something that could automatically format my resume for me? What if it could generate project pages for my website too? And what if it was able to tune a resume based on a job description?

### Goals

- Parse and compile data on job experience, projects, education, etc. to a resume.
- Automatically format the resume and create a PDF file.
- Tune a resume based on a job description.
- Generate HTML resume page based on provided data.
- Generate an entire website.

### Description

This program is really several projects in one. It's capable of taking data formatted as json all the way to a website or PDF with filtering using embedding and text-ranking models based on a job description.

I'm also working on some level of integration with my project management suite to automate more tasks. Like automatically updating my website every week.

#### Data management

The data for the program is a collection of json files. They're simply loaded to C# records that are 1-1 to the json.

#### Model Creation

All the data is combined into a single collection that allows for searching by label and type. A factory method uses the data collection to create models of the data.

The models give data more structure, and are easier to work with in some ways.

Every model implements the IModel interface which provides a generic method that a factory can visit to generate files.

For a resume, the models are

- ResumeModel  
  Contains a Header and Body
- ResumeHeaderModel  
  For name, contact info, links, etc.
- ResumeBodyModel  
  Contains a collection of ResumeSegmentModels
- ResumeSegmentModel  
  Has a title and a collection of ResumeEntryModels
  These are, for example, the "Technical Skills" or "Job Experience" parts of the resume
- ResumeEntryModel  
  Has a title, location, start and end date, description, etc
  This is what each individual job would be, for example

There are other models for the website generation, like a navigation links model

#### Filtering with AI

The main goal of filtering is to use the most relevant set of entries (jobs, projects, and volunteer experiences) while maintain diversity of bullet points and entries.

This is done by using two models:

- A cross encoder  
  Used to rank how relevant a given entry is to the job.
  Slow, but high quality results

- A text embedder  
  Used to check how similar entries are to each other with cosine similarity
  Less accuracy but faster to run, and more importantly, each embedding is independent so only n embeddings have to be created instead of n^2

1. All entries are collected into a single list, and each is ranked against the job description.
2. The highest entry is added to the resume.
3. Each entry not in the resume is checked against each entry in the resume for similarity, and its total score is updated based on its "uniqueness".
4. The highest ranked entry is added to the resume.
5. If the target number of entries in a given segment, all entries under that segment are removed from the list.
6. Repeat 3 - 5 until all targets are filled.
7. Repeat for bullet points.

#### Transforming Models to Files

I wanted to use the same models for creating PDFs, HTML, and whatever else I end up using this for.

My solution was a generic ViewFactory that could be given strategies to transform models into the form I wanted.

The factory implementation is essentially just a container for these strategies and helpers (e.g. an html style manager). The strategies themselves determine what is created by the factory. The ViewStrategy abstract class contains a single method BuildView that takes in a model and the factory (for nested models). Each strategy has a specific type of model it can transform, but all the strategies in a factory return the same thing.

#### PDF Creation

I use QuestPDF for PDF creation.

The view strategies for PDF generation output and Action\<IContainer\> because that was the best for working with the QuestPDF API. I have a few different strategies that I can swap out for different formatting.

#### HTML Website Creation

I wrote an HTML library for this project. It includes file management, linking pages with anchor elements, some styling support, many HTML elements, and website generation.

The view strategies for HTML output an IHtmlItem, which is just something that can be turned into an HTML string.

## Future Plans

- Integrate with project management tools
  - Maintain a richer project database with more information that is gathered by the crawler
  - Automatically generate new bullet points from readmes with the Gemini entry_generator
- Build out the website generation part so I can use this to build my personal website

## Usage

### Building

```bash
git clone https://github.com/Penguinmaster2001/ResumeGenerator.git
cd ResumeGenerator
dotnet build -c release src/ProjectLogging/ProjectLogging.csproj
./src/ProjectLogging/bin/release/net9.0/ProjectLogging
```

### Json Formats

- Personal info:

```json
{
    "Name": "",
    "PhoneNumber": "",
    "Email": "",
    "Location": "",
    "URLs": [ "example.com" ] "https only, but do not include the https:// part"
}
```

- Jobs

```json
[
    {
        "Company": "",
        "Position": "",
        "ShortDescription": "",
        "Points": [ "" ],
        "Skills": [ { "Category": "", "Name": "" } ],
        "Location": "",
        "StartDate": "yyyy-mm-dd",
        "EndDate": "yyyy-mm-dd" or null for present
    }
]
```

- Projects

```json
[
    {
        "Title": "",
        "ShortDescription": "",
        "Points": [ "" ],
        "Skills": [ { "Category": "", "Name": "" } ],
        "Location": "",
        "StartDate": "yyyy-mm-dd",
        "EndDate": "yyyy-mm-dd" or null for present
    }
]
```

- Volunteer / Extracurricular

```json
[
    {
        "Organization": "",
        "Position": "",
        "ShortDescription": "",
        "Points": [ "" ],
        "Skills": [ { "Category": "", "Name": "" } ],
        "Location": "",
        "StartDate": "yyyy-mm-dd",
        "EndDate": "yyyy-mm-dd" or null for present
    }
]
```

- Education

```json
[
    {
        "School": "",
        "Degree": "",
        "ShortDescription": "" or null,
        "Points": [ "" ],
        "Skills": [ { "Category": "", "Name": "" } ],
        "Location": "",
        "StartDate": "yyyy-mm-dd",
        "EndDate": "yyyy-mm-dd" or null for present
    }
]
```

- Courses

```json
{
    "Category 1": [ "" ],
    "Category 2": [ "" ],
    ...
}
```

- Hobbies

```json
{
    "Category 1": [ "" ],
    "Category 2": [ "" ],
    ...
}
```

- Skills

Note: The skills from education, jobs, projects, etc. will also be added to the resume, but only skills under categories listed here

```json
{
    "Category 1": [ "" ],
    "Category 2": [ "" ],
    ...
}
```

- Pro tip 1: Your local LLM will be more than willing to collect all the fields of your current resume(s) into json files

- Pro tip 2: Your local LLM will likely hallucinate quite a bit while it does that, so verify the output

## License

This project is licensed under the [MIT License](LICENSE).

## Features

| name | status |
|-|-|
| Make better use of async operations | planned |
| Clean up the main method | planned |
| use a better ranking system that includes uniqueness | in progress |
| filter resume entries using AI | done |
| compile data for resume generation | done |
| generate a pdf | done |
| generate an html webpage | done |

## Built With

- C#
- .NET
- QuestPDF
- markdown
- VS Code
- HuggingFace

> Anthony Cieri [anthony.cieri@hotmail.com](mailto:anthony.cieri@hotmail.com) [anthonycieri.com](https://anthonycieri.com)
