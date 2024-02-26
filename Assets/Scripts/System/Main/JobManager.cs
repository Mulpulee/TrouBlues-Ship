using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class JobManager
{
    private static Dictionary<JobType, Job> m_jobs;
    public static Job GetJob(JobType type) { return m_jobs[type]; }

    public static void SetJobs()
    {
        Job[] objects = Resources.LoadAll<Job>("ScriptableObject/Job");

        m_jobs = new Dictionary<JobType, Job>();
        foreach (Job obj in objects)
        {
            m_jobs.Add(obj.Type, obj);
        }

        m_jobs[JobType.Captain].SpecialSkill = new CaptainSkill();
        m_jobs[JobType.Engineer].SpecialSkill = new EngineerSkill();
        m_jobs[JobType.Medic].SpecialSkill = new MedicSkill();
        m_jobs[JobType.Janitor].SpecialSkill = new JanitorSkill();
        m_jobs[JobType.Controller].SpecialSkill = new ControllerSkill();
    }
}
