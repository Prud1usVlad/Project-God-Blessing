using Assets.Scripts.ScriptableObjects.Hub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.SkillSystem.Ui
{
    public class SecretsPostingWindow : DialogueBox
    {
        public GameProgress gameProgress;
        public SecretSkill skill;

        public ListViewController secretsView;
        public GameObject noSecretsText;

        [Header("Info panel")]
        public TextMeshProUGUI secretName;
        public TextMeshProUGUI secretDescription;
        public TextMeshProUGUI secretPublishedText;
        public TextMeshProUGUI secretNation;
        public TextMeshProUGUI secretIsPublished;
        public Button publishButton;
        public GameObject messageBox;

        public GameObject infoPanel;

        public override bool InitDialogue()
        {
            var inited = base.InitDialogue();

            if (inited)
            {
                UpdateView();

                infoPanel.SetActive(false);
            }

            return inited;
        }

        public void UpdateView()
        {
            var skills = gameProgress
                .skillSystem.learnedSkills
                .Where(s => s.type == SkillType.Secret);

            if (skills.Any()) 
            {
                secretsView.InitView(skills.Cast<object>().ToList(), skills.First());
                secretsView.selectionChanged += UpdateInfo;
                UpdateInfo();
                noSecretsText.SetActive(false);
            }
            else
            {
                noSecretsText.SetActive(true);
            }
        }

        public void UpdateInfo()
        {
            skill = secretsView.Selected as SecretSkill;

            if (skill != null) 
            {
                infoPanel.SetActive(true);
                secretName.SetText(skill.skillName);
                secretDescription.SetText(skill.description);
                secretNation.SetText(Enum.GetName(typeof(NationName), skill.nation));
                
                if (skill.isPublished)
                {
                    secretIsPublished.SetText("Published");
                    secretIsPublished.color = Color.red;
                    secretPublishedText.SetText(skill.publishMessage);
                    publishButton.gameObject.SetActive(false);
                }
                else
                {
                    secretIsPublished.SetText("Not published");
                    secretIsPublished.color = Color.green;
                    secretPublishedText.SetText("");
                    publishButton.gameObject.SetActive(true);
                }
            }
            else
            {
                infoPanel.SetActive(false);
            }
        }

        public void OnPublish()
        {
            if (skill != null)
            {
                skill.OnPublish();
                var box = Instantiate(messageBox, transform)
                    .GetComponent<DialogueBox>();

                box.ignoreConstraints = true;
                box.header = $"The secret \"{skill.skillName}\" is published";
                box.body = skill.publishMessage;
                modalManager.DialogueOpen(box);

                UpdateView();
            }
        }

    }
}
