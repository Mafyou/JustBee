using JustBeeWeb.Options;
using JustBeeWeb.Serialization;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace JustBeeWeb.Services;

public class EmailService
{
    private readonly HttpClient _httpClient;
    private readonly BrevoOptions _brevoOptions;

    public EmailService(HttpClient httpClient, IOptions<BrevoOptions> brevoOptions)
    {
        _httpClient = httpClient;
        _brevoOptions = brevoOptions.Value;

        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Add("api-key", _brevoOptions.APIKey);
    }

    public async Task<bool> EnvoyerEmailVerificationAsync(string email, string pseudo, string token, string baseUrl)
    {
        try
        {
            var lienVerification = $"{baseUrl}/VerifierEmail?token={token}";

            var emailData = new EmailData
            {
                Sender = new EmailSender { Name = "Plan B Démocratie", Email = "contact@mafyouit.tech" },
                To = [new EmailRecipient { Email = email, Name = pseudo }],
                Subject = "🐝 Vérification de votre alvéole citoyenne - Plan B",
                HtmlContent = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <div style='background: linear-gradient(135deg, #FFD700, #FFA500); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;'>
                            <h1 style='color: #8B4513; margin: 0; font-size: 28px;'>🍯 Bienvenue dans la Ruche Démocratique</h1>
                            <p style='color: #8B4513; margin: 10px 0 0 0; font-size: 16px;'>Plan B - Démocratie Participative</p>
                        </div>
                        
                        <div style='background: #ffffff; padding: 30px; border-radius: 0 0 10px 10px; box-shadow: 0 4px 6px rgba(0,0,0,0.1);'>
                            <h2 style='color: #8B4513; margin-bottom: 20px;'>Bonjour <strong>{pseudo}</strong> ! 🐝</h2>
                            
                            <p style='font-size: 16px; line-height: 1.6; color: #333;'>
                                Merci de rejoindre notre essaimage démocratique ! Votre alvéole citoyenne a été créée avec succès.
                            </p>
                            
                            <div style='background: #FFF8DC; padding: 20px; border-left: 4px solid #FFD700; margin: 20px 0; border-radius: 5px;'>
                                <p style='margin: 0; color: #8B4513; font-weight: bold;'>
                                    🍯 Pour activer votre alvéole et apparaître sur la carte des ruches, 
                                    veuillez confirmer votre adresse email en cliquant sur le bouton ci-dessous :
                                </p>
                            </div>
                            
                            <div style='text-align: center; margin: 30px 0;'>
                                <a href='{lienVerification}' 
                                   style='background: linear-gradient(135deg, #FFD700, #FFA500); 
                                          color: #8B4513; 
                                          padding: 15px 30px; 
                                          text-decoration: none; 
                                          border-radius: 25px; 
                                          font-weight: bold; 
                                          font-size: 16px; 
                                          display: inline-block;
                                          box-shadow: 0 4px 6px rgba(0,0,0,0.1);'>
                                    ✅ Vérifier mon Email
                                </a>
                            </div>
                            
                            <div style='background: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                                <h3 style='color: #8B4513; margin-top: 0;'>🔒 Pourquoi cette vérification ?</h3>
                                <ul style='color: #666; margin: 0; padding-left: 20px;'>
                                    <li>Garantir l'authenticité des alvéoles citoyennes</li>
                                    <li>Assurer la sécurité de notre ruche démocratique</li>
                                    <li>Permettre les notifications importantes</li>
                                    <li>Respecter les principes de transparence du Plan B</li>
                                </ul>
                            </div>
                            
                            <hr style='border: none; border-top: 1px solid #eee; margin: 30px 0;'>
                            
                            <p style='font-size: 14px; color: #666; text-align: center; margin: 0;'>
                                🐸 <em>Comme les grenouilles, sentinelles de l'écosystème, nous veillons sur la santé de notre démocratie participative.</em><br>
                                <strong>Plan B - Démocratie Participative</strong><br>
                                Une innovation démocratique expérimentale dans le cadre constitutionnel français
                            </p>
                        </div>
                    </div>
                "
            };

            var json = JsonSerializer.Serialize(emailData, EmailSerializationContext.Default.EmailData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.brevo.com/v3/smtp/email", content);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            // Log l'erreur (ici on pourrait utiliser ILogger)
            Console.WriteLine($"Erreur envoi email: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> EnvoyerEmailVerificationAlveoleAsync(string email, string nomAlveole, string ville, string token, string baseUrl)
    {
        try
        {
            var lienVerification = $"{baseUrl}/VerifierAlveole?token={token}";

            var emailData = new EmailData
            {
                Sender = new EmailSender { Name = "Plan B Démocratie", Email = "contact@mafyouit.tech" },
                To = [new EmailRecipient { Email = email, Name = nomAlveole }],
                Subject = $"🐝 Vérification de votre alvéole '{nomAlveole}' - Plan B",
                HtmlContent = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <div style='background: linear-gradient(135deg, #FFD700, #FFA500); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;'>
                            <h1 style='color: #8B4513; margin: 0; font-size: 28px;'>🍯 Nouvelle Alvéole Créée</h1>
                            <p style='color: #8B4513; margin: 10px 0 0 0; font-size: 16px;'>Plan B - Démocratie Participative</p>
                        </div>
                        
                        <div style='background: #ffffff; padding: 30px; border-radius: 0 0 10px 10px; box-shadow: 0 4px 6px rgba(0,0,0,0.1);'>
                            <h2 style='color: #8B4513; margin-bottom: 20px;'>Alvéole <strong>{nomAlveole}</strong> 🐝</h2>
                                
                            <div style='background: #FFF8DC; padding: 20px; border-radius: 10px; margin: 20px 0;'>
                                <h3 style='color: #8B4513; margin-top: 0;'>📋 Détails de votre alvéole :</h3>
                                <ul style='color: #666; margin: 0; padding-left: 20px;'>
                                    <li><strong>Nom :</strong> {nomAlveole}</li>
                                    <li><strong>Ville :</strong> {ville}</li>
                                    <li><strong>Email de contact :</strong> {email}</li>
                                </ul>
                            </div>
                            
                            <p style='font-size: 16px; line-height: 1.6; color: #333;'>
                                Votre alvéole a été créée avec succès dans la ruche démocratique ! 
                                Pour qu'elle apparaisse sur la carte et soit visible par les autres abeilles citoyennes, 
                                veuillez confirmer votre adresse email.
                            </p>
                            
                            <div style='text-align: center; margin: 30px 0;'>
                                <a href='{lienVerification}' 
                                   style='background: linear-gradient(135deg, #FFD700, #FFA500); 
                                          color: #8B4513; 
                                          padding: 15px 30px; 
                                          text-decoration: none; 
                                          border-radius: 25px; 
                                          font-weight: bold; 
                                          font-size: 16px; 
                                          display: inline-block;
                                          box-shadow: 0 4px 6px rgba(0,0,0,0.1);'>
                                    🚀 Activer mon Alvéole
                                </a>
                            </div>
                            
                            <div style='background: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                                <h3 style='color: #8B4513; margin-top: 0;'>🏛️ À propos des alvéoles citoyennes :</h3>
                                <p style='color: #666; margin: 0;'>
                                    Les alvéoles sont des espaces de démocratie participative où les citoyens peuvent 
                                    s'organiser, débattre et proposer des solutions pour leur territoire. 
                                    Chaque alvéole contribue à construire une démocratie plus proche des citoyens.
                                </p>
                            </div>
                            
                            <hr style='border: none; border-top: 1px solid #eee; margin: 30px 0;'>
                            
                            <p style='font-size: 14px; color: #666; text-align: center; margin: 0;'>
                                🤝 <em>Ensemble, construisons une démocratie participative plus proche des citoyens</em><br>
                                <strong>Plan B - Démocratie Participative</strong><br>
                                Innovation démocratique expérimentale - Carcès, Var (83)
                            </p>
                        </div>
                    </div>
                "
            };

            var json = JsonSerializer.Serialize(emailData, EmailSerializationContext.Default.EmailData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.brevo.com/v3/smtp/email", content);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur envoi email alvéole: {ex.Message}");
            return false;
        }
    }
}