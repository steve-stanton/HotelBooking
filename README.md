# Approach

* Keep it simple
  - no caching
  - no logging
  - no traces
  - single project

* Stick to the requirements
* Use issues to document questions / suggestions

# API Requirements

Provide endpoints for the following:

* Find a hotel based on its name
* Find available rooms between two dates for a given number of people
* Book a room
* Find booking details based on a booking reference number

Authentication not required

It would be good to have a bit more background info, such as:

* Who is going to consume the API?
* Are these requirements cast in stone?
* Are there additional endpoints to come?
* What attributes will the database hold for hotels and rooms?

If I assume that the API is going to be consumed by a frontend app, I would question
whether the requirement to find a hotel by name has much value. Given the requirements,
my hotel entity only needs to hold a hotel name, and some sort of unique (and permanent) ID.
I could invent other fields (I imagine that location is essential), but if I'm aiming to
stick to requirements, I shouldn't really invent stuff. Which means that when someone calls the endpoint
to find a hotel by name, the only extra item of information they'll get is the hotel's internal
ID... but should the API even expose that to the caller?

There also needs to be a more precise specification of what's required in terms of the
endpoint to find booking details. What "details" are we talking about here? I already
have a sense that I will be dealing with a Booking entity that is associated with a
Room, which is associated in turn with a Hotel. The booking details as far as an end-user
is concerned would likely combine elements of all 3 entities (with IDs converted into
names).

For the sake of this tech test, I think it's probably ok to take more of a developer's
perspective. So the endpoint to find a hotel will return the entire Hotel entity (including
its ID), while the endpoint to find booking details will return just the Booking entity.

# Environment

## Database

A relational database will deal with the requirements just fine.
And with Azure in the mix, the obvious choice would be to work with their own SQL database.

I did briefly consider a Cosmos database, but couldn't think of any advantages.

# Implementation Progress

I started out by identifying tasks in the challenge specification, and added them as github issues.
Each issue is pretty light on detail, but good enough for a tech test where I'm only really looking to
document how I approach things.

I also signed up for a free Azure subscription, and confirmed that it doesn't impose any restrictions
on what I expect to do.

In terms of coding, I did consider if I should work with git branches for each task, and only merge to
main upon task completion. But given that it's just me doing the coding, and it's only expected to
take "2 or 3 hours" (hmmm), I figured that merging directly to main is fine.

To see my progress on each task, have a look at the github issues. I'll assign the issue to myself
when I start it. And mark it as closed when I finish the work. I'll add comments to each issue if
I discover something I hadn't thought of.

# Challenges

* Not entirely sure about the best way to document the API these days. I stuck with swagger because
I've used that in the past. But I seem to recall that there may now be alternatives - something to
look into perhaps. I did have some difficulty getting the swagger pages to pick up xml code comments
when using MinimalApi. I switched back to using a Controller class, since that's what I usually see,
and xml code comments work in that case.
<br/><br/>
* I just started looking into setting up my Azure SQL database. Setting things up with the right
permissions is a bit of a maze. It looks like things have moved on since I last had to think about
this, so I need to do a bit of a catchup to learn current best practice For the purpose of the tech test,
I think the easiest would be just to allow SQL logins.

# Retrospective

I've now had a few days to think about the requirements, and I start to
wonder if I was right to assume that the API is meant to be called by a frontend app.
My clue is that we now have a `/booking/find-rooms` endpoint that returns a list of
available rooms. What is my end-user going to do with that?

Real booking systems do not present users with a list of rooms that they can select
from. When I book a room at a hotel, I would expect them to confirm that they have
the space to accommodate me, but I doubt that they would immediately allocate a room.
I think it's more likely that they would do that much closer to my arrival date.
That would give the hotel the ability to optimize room allocation based on confirmed
demand. For example, the hotel might have a policy to only open up a specific floor
once all other floors have been fully occupied (perhaps to save on heating bills).

In that scenario, the requirement to return all available rooms makes perfect sense,
but it would be called from a room allocation service rather than a user-facing
frontend. That being the case, I would probably need to revisit my database model
because each booking ID currently includes the allocated room ID. My `Booking`
entity is more like an `AllocatedRoom` entity.

In this instance, I have a fair idea what a real booking system might require because
booking a hotel room is something that everyone has done at some point. More
generally, developers will need to gain some domain knowledge before embarking on
a project. I would think that this can best be achieved by involving developers
while building the business case for a project.
