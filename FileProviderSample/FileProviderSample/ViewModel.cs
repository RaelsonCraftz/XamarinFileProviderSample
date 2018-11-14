using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Input;
using Syncfusion.DocIO.DLS;
using Xamarin.Forms;

namespace FileProviderSample
{
    public class ViewModel
    {

        public ICommand CreateWord { get; set; }

        public ViewModel()
        {
            CreateWord = new Command(CreationWord);
        }

        async void CreationWord()
        {
            WordDocument localdocumento = new WordDocument();

            IWSection localSection = localdocumento.AddSection();

            localSection.AddParagraph();

            IWParagraph localParagraph = localSection.AddParagraph();
            localParagraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
            IWTextRange textoTitulo = localParagraph.AppendText("ATA DE REUNIÃO - Projetos");

            textoTitulo.CharacterFormat.TextColor = Syncfusion.Drawing.Color.ForestGreen;
            textoTitulo.CharacterFormat.Font = new Syncfusion.Drawing.Font("Times New Roman", 18);
            
            //Lista de informações e pessoas
            IWTextRange textoDescricao = localParagraph.AppendText("Reunião do Engenheiros Sem Fronteiras - Núcleo João Pessoa - Diretoria");

            IWTextRange textoSubDescricao = localParagraph.AppendText("A seguinte pauta foi discutida:");

            MemoryStream arquivostream = new MemoryStream();
            arquivostream.Position = 0;

            localdocumento.Save(arquivostream, Syncfusion.DocIO.FormatType.Docx);

            localdocumento.Close();

            await DependencyService.Get<ISave>().Save("XamarinTest" + ".docx", "application/msword", arquivostream);
        }

    }
}
