# Resume Generator

A program that generates a resume pdf from json files

## Table of Contents

- [Building](#building)
- [Usage](#usage)
- [License](#license)

## Building

```bash
git https://github.com/Penguinmaster2001/ResumeGenerator.git
cd ResumeGenerator
dotnet build -c release src/ProjectLogging/ProjectLogging.csproj
dotnet run --project src/ProjectLogging/ProjectLogging.csproj
```

## Usage

Json formats:

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
