
using System.Text.Json;
using ProjectLogging.Data;
using ProjectLogging.Models.Website;
using ProjectLogging.Views.Html;
using ProjectLogging.Views.ViewCreation;
using ProjectLogging.WebsiteGeneration.GenerationContext;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation;
using ProjectLogging.WebsiteGeneration.HtmlRepresentation.HtmlElements;



namespace ProjectLogging.WebsiteGeneration;



public static class WebsiteGenerator
{
    public static async Task<Website> GenerateWebsiteAsync(string outDir, IDataCollection data, WebsiteGenerationSettings settings, List<ProjectReadme> projectReadmes)
    {
        var projects = projectReadmes.Select(p => new ProjectCard(p)).ToList();

        var fileOrganizer = new WebsiteFileOrganizer
        {
            RootDirectory = outDir,
        };

        var templateSettings = await JsonSerializer.DeserializeAsync<TemplateSettings>(File.OpenRead(settings.TemplateSettingsPath));

        var templateManager = await LoadTemplatesAsync(templateSettings!, settings.TemplatesPath);

        var htmlStyleManager = new HtmlStyleManager(settings.Styles.ToDictionary(kvp => kvp.Key, kvp => Path.Combine(settings.StylesPath, kvp.Value)));

        var viewFactory = new ViewFactory<IHtmlItem>();
        SetUpFactory(viewFactory, htmlStyleManager, new PageLinker(fileOrganizer), templateManager, templateSettings!);

        var website = new Website(fileOrganizer);

        var head = templateManager.Create("head", new Dictionary<string, object>
        {
            {"meta_description", "This is a meta description description"},
            {"page_title", "Project Page"},
            {"css_paths", new List<string> {htmlStyleManager.StylePaths["base"]}},
        });
        var header = templateManager.Create("header", null);
        var footer = templateManager.Create("footer", new Dictionary<string, object>
        {
            {"year", DateTime.Now.Year.ToString()},
        });

        var personalInfo = data.GetData<PersonalInfo>(data.DataConfig.PersonalInfo.Title);

        website.Pages.Add(new TemplatePage
        {
            Title = "home",
            Head = templateManager.Create("head", new Dictionary<string, object>
            {
                {"meta_description", "This is a meta description description"},
                {"page_title", "Project Page"},
                {"og_image", "Project Page"},
                {"canonical_url", "Project Page"},
                {"css_paths",
                    new List<string>
                    {
                        htmlStyleManager.StylePaths["base"],
                        htmlStyleManager.StylePaths["home"]
                    }
                },
            }),
            Header = header,
            Body = templateManager.Create("home", IDataCache.Create(data.GetData<Dictionary<string, object>>("main_page"))),
            Footer = footer,
        });

        website.Pages.AddRange(await CreateProjectPages(head, header, footer, projectReadmes, viewFactory));

        return website;
    }



    private static void SetUpFactory(
        ViewFactory<IHtmlItem> viewFactory,
        IHtmlStyleManager htmlStyleManager,
        IPageLinker pageLinker,
        ITemplateManager templateManager,
        TemplateSettings templateSettings)
    {
        viewFactory.AddStrategy(new TemplateHtmlStrategy<ProjectInfo>(templateSettings.TemplateUses["ProjectInfo"]));
        viewFactory.AddStrategy(new TemplateHtmlStrategy<FooterInfo>(templateSettings.TemplateUses["FooterInfo"]));
        viewFactory.AddStrategy<ResumeSegmentHtmlStrategy>();
        viewFactory.AddStrategy<ResumeHeaderHtmlStrategy>();
        viewFactory.AddStrategy<ProjectCardHtmlStrategy>();
        viewFactory.AddStrategy<ResumeEntryHtmlStrategy>();
        viewFactory.AddStrategy<ResumeBodyHtmlStrategy>();
        viewFactory.AddStrategy<ResumeHtmlStrategy>();
        viewFactory.AddStrategy<NavLinksStrategy>();

        viewFactory.AddHelper(htmlStyleManager);
        viewFactory.AddHelper(templateManager);
        viewFactory.AddHelper(pageLinker);

        viewFactory.AddPostAction((htmlItem, factory) =>
            {
                if (htmlItem is IHtmlElement htmlElement)
                {
                    factory.GetHelper<IHtmlStyleManager>().ApplyStyle(htmlElement);
                }
            });
    }



    private static async Task<List<IHtmlPage>> CreateProjectPages(
        IHtmlItem head,
        IHtmlItem header,
        IHtmlItem footer,
        List<ProjectReadme> projectReadmes,
        IViewFactory<IHtmlItem> viewFactory)
    {
        var styleManager = viewFactory.GetHelper<IHtmlStyleManager>();

        var projectCards = new List<ProjectCard>();

        var projectPages = await Task.WhenAll(projectReadmes.Select(async r =>
        {
            var card = new ProjectCard(r);
            projectCards.Add(card);

            var info = await ProjectInfo.CreateFromCardAsync(card);

            return new TemplatePage
            {
                Title = info.ProjectTitle,
                Head = head,
                Header = header,
                Body = info.CreateView(viewFactory),
                Footer = footer,
            };
        }));

        // var projectCardPage = new HtmlPageBuilder("page1", "styles/stylesNew.css")
        //     .AddHeader(new NavLinksModel(["page2"]).CreateView(viewFactory))
        //     .AddBody(IHtmlElement.Div(projectCards.Select(p => p.CreateView(viewFactory))))
        //     .AddFooter(new RawTagElement(HtmlTag.HtmlTags.Paragraph, "this is the footer"))
        //     .Build();

        // return [.. projectPages.Append(projectCardPage)];
        return [.. projectPages];
    }



    private static async Task<ITemplateManager> LoadTemplatesAsync(TemplateSettings templateSettings, string templateDir)
    {
        var files = Directory.EnumerateFiles(templateDir, "*", SearchOption.TopDirectoryOnly).ToArray();

        var tasks = files.Select(async path =>
        {
            var template = await File.ReadAllTextAsync(path).ConfigureAwait(false);
            var templateName = Path.GetFileNameWithoutExtension(path);
            templateName = templateSettings.TemplateNames.GetValueOrDefault(templateName, templateName);

            Console.WriteLine(templateName);
            return (templateName, template);
        }).ToArray();

        var results = await Task.WhenAll(tasks).ConfigureAwait(false);
        var templateNames = results.ToDictionary(t => t.templateName, t => t.template);

        return new TemplateManager(templateNames);
    }
}
