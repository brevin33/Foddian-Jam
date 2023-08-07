using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI leaderBoardText;

    [SerializeField]
    GameObject board;

    [SerializeField]
    End end;

    LootLockerLeaderboardMember[] top5;

    LootLockerLeaderboardMember[] surroundingPlacements;

    string memberID;

    bool error;

    public void close()
    {
        board.SetActive(false);
    }

    public void setPlayer(string name)
    {
        memberID = name;
    }
    public void showLeaderBoardText()
    {
        error = false;
        top5 = null;
        surroundingPlacements = null;
        int leaderboardID = 16662;
        int count = 5;
        string s = "";

        LootLockerSDKManager.GetScoreList(leaderboardID, count, 0, (response) =>
        {
            if (response.statusCode == 200)
            {
                top5 = response.items;
            }
            else
            {
                error = true;
            }
        });


        LootLockerSDKManager.GetMemberRank(leaderboardID, memberID, (response) =>
        {
            if (response.statusCode == 200)
            {
                int rank = response.rank;
                int count = 3;
                int after = Mathf.Max( rank - 2,0);

                LootLockerSDKManager.GetScoreList(leaderboardID, count, after, (response) =>
                {
                    if (response.statusCode == 200)
                    {
                        surroundingPlacements = response.items;
                    }
                    else
                    {
                        error = true;
                    }
                });
            }
            else
            {
                Debug.Log("failed: " + response.Error);
                error = true;
            }
        });

        StartCoroutine(changeText());
    }

    public void submitScore(int score)
    {
        int leaderboardID = 16662;
        LootLockerSDKManager.SubmitScore(memberID, score, leaderboardID, (response) =>
        {
            if (response.statusCode == 200)
            {
                end.nameSubmited = true;
                Debug.Log("Successful");
            }
            else
            {
                end.nameSubmited = true;
                Debug.Log("failed: " + response.Error);
            }
        });
    }


    IEnumerator changeText()
    {
        yield return new WaitUntil(gotResponce);
        if (error)
        {
            leaderBoardText.text = "Failed to Load";
        }
        else
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < top5.Length; i++)
            {
                stringBuilder.AppendLine(top5[i].rank.ToString() + "    " + top5[i].member_id + " " + top5[i].score );
            }
            stringBuilder.AppendLine("...");
            for (int i = 0; i < surroundingPlacements.Length; i++)
            {
                stringBuilder.AppendLine(surroundingPlacements[i].rank.ToString() + "    " + surroundingPlacements[i].member_id + " " + surroundingPlacements[i].score);
            }
            leaderBoardText.text = stringBuilder.ToString();
        }

        board.SetActive(true);
    }


    bool gotResponce()
    {
        return (top5 is not null && surroundingPlacements is not null) || error;
    }
}
