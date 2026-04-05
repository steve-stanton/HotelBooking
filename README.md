# Dev Notes

## Approach

* Keep it simple
  - no caching
  - no logging
  - no traces
  - single project

* Stick to the requirements
* Use issues to document questions / suggestions

## API Requirements

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

## Database Choice

A relational database will deal with this just fine. And with Azure in the mix, the obvious
choice would be to work with their own SQL database.

I did briefly consider a Cosmos database, but couldn't think of any advantages.



